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

namespace Bank
{
	public partial class Bank : Form
	{
		public Bank()
		{
			InitializeComponent();
		}

		readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accounts.json");
		List<Account> AccountList = new List<Account>();

		private void Bank_Load(object sender, EventArgs e)
		{
			toolTip1.Active = true; 
			toolTip1.ShowAlways = true;
			toolTip1.SetToolTip(PassportData, "Формат паспорта: AB1234567 (2 буквы + 7 цифр)");
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

			string[] FullNameList = this.FullName.Text.Split(' ');

			try
			{
				
				if (FullName.Split(' ').Length != 3) throw new Exception("Введите ФИО полностью!");
				if (string.IsNullOrEmpty(FullName)) throw new Exception("Заполните ФИО владельца полностью!");
				if (!PassportData.MaskCompleted) throw new Exception("Паспортные данные должны быть заполненными!");
				if (!this.BirthDate.Checked) { throw new Exception("Введите дату рождения!"); }
				if (!this.Account.MaskCompleted) throw new Exception("Введите номер счета полностью!");
				if (string.IsNullOrEmpty(AccountType)) throw new Exception("Тип счета должен быть выбран!");

				AccountList.Add(new Account(
				Account,
				new Owner(FullName, BirthDate, Passport),
				AccountType,
				Balance,
				OpeningDate,
				isSMS,
				isInternetBanking
				));
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
				.GroupBy(x => x.Number)
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
				using (FileStream fs = new FileStream(FilePath, FileMode.Open))
				{
					List<Account> accounts = JsonSerializer.Deserialize<List<Account>>(fs);

					if (accounts == null || accounts.Count == 0)
						throw new Exception("Файл не содержит корректных данных!");

					foreach (var account in accounts)
					{
						if (!ValidateAccount(account))
						{
							MessageBox.Show($"Ошибка валидации данных для счета {account.Number}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							continue; 
						}

						AccountList.Add(account);
						dataGridView1.Rows.Add(
							account.Owner.FullName,
							account.Owner.PassportData,
							account.Owner.BirthDate.ToShortDateString(),
							account.Number,
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

		private bool ValidateAccount(Account account)
		{
			if (account == null)
				return false;

			if (string.IsNullOrWhiteSpace(account.Owner.FullName) || account.Owner.FullName.Split(' ').Length != 3)
				return false;

			if (string.IsNullOrWhiteSpace(account.Owner.PassportData) || !System.Text.RegularExpressions.Regex.IsMatch(account.Owner.PassportData, @"^[A-Z]{2}\d{7}$"))
				return false;

			int age = DateTime.Now.Year - account.Owner.BirthDate.Year;
			if (account.Owner.BirthDate > DateTime.Now.AddYears(-age)) age--;
			if (age < 18)
				return false;

			if (string.IsNullOrWhiteSpace(account.Number) || account.Number.Length != 8)
				return false;

			if (string.IsNullOrWhiteSpace(account.AccountType))
				return false;

			if (account.Balance < 0)
				return false;

			return true;
		}
	}
}
