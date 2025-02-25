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
				if (string.IsNullOrEmpty(FullName)) throw new Exception("ФИО владельца не должно быть пустым!");
				if (!PassportData.MaskCompleted) throw new Exception("Паспортные данные не должны быть пустыми");
				if (string.IsNullOrEmpty(Account)) throw new Exception("Номер счета должен быть указан");
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

		private void PassportData_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				/*if (string.IsNullOrEmpty)*/

		
			}

			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка заполнения формы: {ex.Message}");
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
			MessageBox.Show("1");

			foreach(var OwnerBudget in OwnerBudgets)
			{
				if (OwnerBudget.Owner != null)
				{
					output += $"{OwnerBudget.Owner} - \t {OwnerBudget.Budget}\n";
				}
			}

			OutputBudget.Text = output;
			MessageBox.Show("2");
		}

		private void label8_Click(object sender, EventArgs e)
		{

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
			using (FileStream fs = new FileStream(FilePath, FileMode.Open))
			{
			
				List<Account> accounts = JsonSerializer.Deserialize<List<Account>>(fs);

				dataGridView1.Rows.Clear();
				AccountList.AddRange(accounts);

				foreach (var account in AccountList)
				{
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


	}
}
