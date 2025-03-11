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
			string AccountNumber = this.AccountNumber.Text;
			string AccountType = this.AccountType.SelectedItem?.ToString();
			double Balance = Convert.ToDouble(this.Balance.Value);
			DateTime OpeningDate = this.Opening.Value;
			bool isSMS = this.SMS.Checked;
			bool isInternetBanking = this.InternetBanking.Checked;

			try
			{
				AccountList.Add(new Account(
				AccountNumber,
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
					AccountList.RemoveAt(AccountList.Count - 1);
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
					AccountList.RemoveAt(AccountList.Count - 1);
					return;
				}

				Account.IncreaseClients();
				FulfilDGV(dataGridView1, AccountList);
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
			const int minimumAge = 14;
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

			Account.ChangeLastAction("Нажата кнопки расчета бюджета клиентов");
		}
	
		private void WriteFile_Click(object sender, EventArgs e)
		{
			using (FileStream fs = new FileStream(FilePath, FileMode.Create))
			{
				var options = new JsonSerializerOptions { WriteIndented = true };
				JsonSerializer.Serialize(fs, AccountList, options);
			}

			MessageBox.Show("Данные успешно записаны в файл!");
			Account.ChangeLastAction("Нажата кнопка записи данных из JSON");

		}

		private void FileRead_Click(object sender, EventArgs e)
		{
			try
			{

				List<Account> accounts;

				var options = new JsonSerializerOptions
				{
					IncludeFields = true 
				};

				using (FileStream fs = new FileStream(FilePath, FileMode.Open))
				{

					accounts = JsonSerializer.Deserialize<List<Account>>(fs, options);
					if (accounts == null || accounts.Count == 0)
						throw new Exception("Файл не содержит корректных данных!");

				}

				foreach (var account in accounts)
				{
					AccountList.Add(account);
					Account.IncreaseClients();
				}

				FulfilDGV(dataGridView1, AccountList);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			Account.ChangeLastAction("Нажата кнопка чтения данных из JSON");
		}


		private void информацияToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Версия приложения: 1.2. \nРазработчик: Романов Игорь Вячеславович");
			Account.ChangeLastAction("Нажата кнопка информации о программе");
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

			Account.ChangeLastAction("Нажата кнопка сохранения результатов поиска");

		}

		private void сохранитьРезультатыСортировкиToolStripMenuItem_Click(object sender, EventArgs e)
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

			Account.ChangeLastAction("Нажата кнопка сохранения результатов сортировки");
		}

		private void HidePanel_Click(object sender, EventArgs e)
		{
			BottomToolPannel.Visible = false;
			отобразитьПанельToolStripMenuItem.Visible = true;
			Account.ChangeLastAction("Нажата кнопка скрытия панели инструментов");
		}

		private void PinPannel_Click(object sender, EventArgs e)
		{
			BottomToolPannel.GripStyle = ToolStripGripStyle.Hidden;
			Account.ChangeLastAction("Нажата кнопка закрепления панели инструментов");
		}

		private void отобразитьПанельToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BottomToolPannel.Visible = true;
			отобразитьПанельToolStripMenuItem.Visible = false;
			Account.ChangeLastAction("Нажата кнопка отобржаения панели инструментов");
		}
		private void UpButton_Click(object sender, EventArgs e)
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

			Account.ChangeLastAction("Нажата кнопка передвижения вверх по таблице");
		}

		private void DownButtonButton_Click(object sender, EventArgs e)
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

			Account.ChangeLastAction("Нажата кнопка передвижения вниз по таблице");
		}

		private void ClearButton_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
			Account.ChangeLastAction("Нажата кнопка очищения таблицы данных");
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
			AccountList.Clear();
			Account.ClearClients();
			Account.ChangeLastAction("Нажата кнопка удаления данных");
		}

		private void SearchButton_Click(object sender, EventArgs e)
		{
			SearchForm searchForm = new SearchForm(AccountList, dataGridView1);
			searchForm.Show();
			Account.ChangeLastAction("Нажата кнопка поиска данных");
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
				MessageBox.Show("Введите данные для сортировки!");
			}

			Account.ChangeLastAction("Нажата кнопка сортировки данных по дате рождения");
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

			Account.ChangeLastAction("Нажата кнопка сортировки данных по ФИО");
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

			Account.ChangeLastAction("Нажата кнопка сортировки данных по номеру счета");
		}

		private void текущееВремяToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var CurrentTime = DateTime.Now;
			MessageBox.Show($"Текущее время: {CurrentTime:HH:mm:ss}");
			Account.ChangeLastAction("Нажата кнопка получений текущего времени");
		}

		private void текущаяДатаToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			var CurrentDate = DateTime.Now;
			MessageBox.Show($"Текущая дата: {CurrentDate:dd:MM:yyyy}");
			Account.ChangeLastAction("Нажата кнопка получений текущей даты");
		}

		private void количествоКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Account.ShowAccountsAmount();
			Account.ChangeLastAction("Нажата кнопка просмотра количества клиентов");
		}

		private void последнееДействиеToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show($"Последнее действие: {Account.LastAction}");
			Account.ChangeLastAction("Нажата кнопка просмотра последнего действия");
		}
	}
}
