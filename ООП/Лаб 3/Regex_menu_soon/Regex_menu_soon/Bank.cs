using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using Regex_menu_soon;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Bank
{
	public partial class Bank : Form
	{
		public Bank()
		{
			InitializeComponent();
		}

		int currentRowIndex = -1; 

		readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accounts.json");
		readonly string SearchPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "search.json");
		readonly string SortPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sort.json");

		List<Account> AccountList = new List<Account>();
		List<Account>SortedAccountList = new List<Account>();

		private void Bank_Load(object sender, EventArgs e)
		{
			toolTip1.Active = true; 
			toolTip1.ShowAlways = true;
			toolTip1.SetToolTip(PassportData, "Формат паспорта: AB1234567 (2 буквы + 7 цифр)");
		}

		public static void FulfilDGV(DataGridView dgv, List<Account> list)
		{
			foreach (var account in list)
			{
				dgv.Rows.Add(
				account.Owner.FullName,
				account.Owner.PassportData,
				account.Owner.BirthDate.ToShortDateString(),
				account.AccountNumber,
				account.AccountType,
				account.Balance,
				account.OpeningDate.ToShortDateString(),
				account.IsSMS ? "+" : "-",
				account.IsInternetBanking ? "+" : "-"
				);
			}
		}

		private void ButtonFinish_Click(object sender, EventArgs e)
		{

			string FullName = this.FullName.Text;
			string Passport = this.PassportData.Text;
			DateTime BirthDate = this.BirthDate.Value;
			string Account = this.Account.Text;
			string AccountType = this.AccountType.SelectedItem?.ToString();
			double Balance = Convert.ToDouble(this.Balance.Value);
			DateTime OpeningDate = this.Opening.Value;
			bool isSMS = this.SMS.Checked;
			bool isInternetBanking = this.InternetBanking.Checked;

			try
			{
				AccountList.Add(new Account(
				Account,
				new Owner(FullName, BirthDate, Passport),
				AccountType,
				Balance,
				OpeningDate,
				isSMS,
				isInternetBanking
				));

				var results = new List<ValidationResult>();
				var lastAccount = AccountList.Last();
				var context = new ValidationContext(lastAccount);


				if (!Validator.TryValidateObject(lastAccount, context, results, true))
				{
					foreach (var error in results)
					{
						MessageBox.Show($"Ошибка валидации: {error.ErrorMessage}");
						
					}
					return;
				}

				var owner = AccountList.Last().Owner;
				var resultsT = new List<ValidationResult>();
				var contextT = new ValidationContext(owner);

				if (!Validator.TryValidateObject(owner, contextT, resultsT, true))
				{
					foreach (var error in resultsT)
					{
						MessageBox.Show($"Ошибка валидации: {error.ErrorMessage}");
					}
					return;
				}

				dataGridView1.Rows.Add(
					FullName,
					Passport,
					BirthDate,
					Account,
					AccountType,
					Balance,
					OpeningDate,
					isSMS ? "+" : "-",
					isInternetBanking ? "+" : "-");
			}

			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка заполнения формы: {ex.Message}");
			}

		}

		private void FullName_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{

				if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != ' ')
				{
					e.Handled = true;
					throw new Exception("ФИО должно состоять только из букв");
				}

			}

			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка заполнения формы: {ex.Message}");
			}
		}
		private void BirthDate_Validating(object sender, CancelEventArgs e)
		{
			int minimumAge = 14;
			DateTime birthDate = BirthDate.Value;
			DateTime minDate = DateTime.Now.AddYears(-minimumAge);
			if (minDate < birthDate)
			{
				MessageBox.Show($"Возраст должен быть не менее {minimumAge} лет.");
			}
		}
		private void Budget_Click(object sender, EventArgs e)
		{
			var OwnerBudgets = AccountList
				.GroupBy(x => x.Owner.FullName)
				.Select(x => new
				{
					Owner = x.Key,
					Budget = x.Sum(a => a.Balance)
				})
				.ToList();

			OutputBudget.Clear();
			string output = "Бюджет всех владельцев: \n";
			foreach(var OwnerBudget in OwnerBudgets)
			{
				if (OwnerBudget.Owner != null)
				{
					output += $"{OwnerBudget.Owner} - \t {OwnerBudget.Budget}\n";
				}
			}

			OutputBudget.Text = output;
		}
	
		private void WriteFile_Click(object sender, EventArgs e)
		{
			using (FileStream fs = new FileStream(FilePath, FileMode.Create))
			{
				var options = new JsonSerializerOptions { WriteIndented = true };
				JsonSerializer.Serialize(fs, AccountList, options);
			}

			MessageBox.Show("Данные успешно записаны в файл!");

		}

		private void FileRead_Click(object sender, EventArgs e)
		{
			try
			{
				var options = new JsonSerializerOptions
				{
					IncludeFields = true 
				};

				using (FileStream fs = new FileStream(FilePath, FileMode.Open))
				{

				
					List<Account> accounts = JsonSerializer.Deserialize<List<Account>>(fs, options);
		
					if (accounts == null || accounts.Count == 0)
						throw new Exception("Файл не содержит корректных данных!");

					

					foreach (var account in accounts)
					{
			
						AccountList.Add(account);
						dataGridView1.Rows.Add(
							account.Owner.FullName,
							account.Owner.PassportData,
							account.Owner.BirthDate.ToShortDateString(),
							account.AccountNumber,
							account.AccountType,
							account.Balance,
							account.OpeningDate.ToShortDateString(),
							account.IsSMS ? "+" : "-",
							account.IsInternetBanking ? "+" : "-"
						);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		private void информацияToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Версия приложения: 1.0. \nРазработчик: Романов Игорь Вячеславович");
		}

		private void сохранитьРезультатыПоискToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SortedAccountList.Count > 0)
			{
				string json = JsonSerializer.Serialize(SortedAccountList);
				File.WriteAllText(SearchPath, json);
				MessageBox.Show("Данные успешно записаны в файл!");
			}
			else
			{
				MessageBox.Show("Введите данные для поиска для записи в файл!");
			}

		}

		private void сохранитьРезлуьтатыСортировкиToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SortedAccountList.Count > 0)
			{
				string json = JsonSerializer.Serialize(SortedAccountList);
				File.WriteAllText(SortPath, json);
				MessageBox.Show("Данные успешно записаны в файл!");
			}
			else
			{
				MessageBox.Show("Введите данные для поиска для записи в файл!");
			}
		}

		private void HidePanel_Click(object sender, EventArgs e)
		{
			BottomToolPannel.Visible = false;
		}

		private void PinPannel_Click(object sender, EventArgs e)
		{
			BottomToolPannel.GripStyle = ToolStripGripStyle.Hidden;
		}

		private void отобразитьПанельToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BottomToolPannel.Visible = true;
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.Count == 0)
			{
				MessageBox.Show("В таблице нет данных!");
				return;
			}

			if (currentRowIndex < dataGridView1.Rows.Count - 1)
			{
				currentRowIndex++; 
				dataGridView1.ClearSelection(); 
				dataGridView1.Rows[currentRowIndex].Selected = true;
				dataGridView1.FirstDisplayedScrollingRowIndex = currentRowIndex; 
			}
		}

		private void BackButton_Click(object sender, EventArgs e)
		{
			if (dataGridView1.Rows.Count == 0)
			{
				MessageBox.Show("В таблице нет данных!");
				return;
			}

			if (currentRowIndex > 0)
			{
				currentRowIndex--;
				dataGridView1.ClearSelection();
				dataGridView1.Rows[currentRowIndex].Selected = true;
				dataGridView1.FirstDisplayedScrollingRowIndex = currentRowIndex;
			}
		}

		private void ClearButton_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
			AccountList.Clear();

		}

		private void SearchButton_Click(object sender, EventArgs e)
		{
			SearchForm searchForm = new SearchForm(AccountList, dataGridView1);
			searchForm.Show();
		}

		private void датаРожденияToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (AccountList.Count > 0)
			{
				SortedAccountList = AccountList.OrderBy(a => a.Owner.BirthDate).ToList();
				dataGridView1.Rows.Clear();
				FulfilDGV(dataGridView1, SortedAccountList);
			}
			else
			{
				MessageBox.Show("Введите данные для сортировки для записи в файл!");
			}
		}

		private void фИОToolStripMenuItem1_Click(object sender, EventArgs e)
		{

			if (AccountList.Count > 0)
			{
				SortedAccountList = AccountList.OrderBy(a => a.Owner.FullName).ToList();
				dataGridView1.Rows.Clear();
				FulfilDGV(dataGridView1, SortedAccountList);
			}
			else
			{
				MessageBox.Show("Введите данные для сортировки для записи в файл!");
			}
		}

		private void номеруСчетаToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (AccountList.Count > 0)
			{
				SortedAccountList = AccountList.OrderBy(a => a.AccountNumber).ToList();
				dataGridView1.Rows.Clear();
				FulfilDGV(dataGridView1, SortedAccountList);
			}
			else
			{
				MessageBox.Show("Введите данные для сортировки для записи в файл!");
			}
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void текущееВремяToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void количествоКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show($"Количество клиентов: {Account.}")
		}
	}
}
