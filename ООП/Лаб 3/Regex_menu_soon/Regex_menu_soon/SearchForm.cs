using Bank;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank
{
	public partial class SearchForm : Form
	{

		List<Account> AccountList = new List<Account>();
		DataGridView dataGridView1 = new DataGridView();
		public SearchForm(List<Account> accounts, DataGridView dataGridView)
		{
			InitializeComponent();
			AccountList = accounts;
			dataGridView1 = dataGridView;
		}

		private void SearchButton_Click(object sender, EventArgs e)
		{
			string Criteria = CriteriaBox.SelectedItem?.ToString();
			string Query = QueryInput.Text.Trim();			

			if (string.IsNullOrEmpty(Criteria))
			{
				MessageBox.Show("Выберите критерий для поиска");
				return;
			}

			if (string.IsNullOrEmpty(Criteria))
			{
				MessageBox.Show("Выберите критерий для поиска");
				return;
			}

			try
			{
				Regex regex = new Regex(Query, RegexOptions.IgnoreCase);
				var found = AccountList.Where(acc =>
				(Criteria == "Номер счета" & regex.IsMatch(acc.AccountNumber)) ||
				(Criteria == "ФИО" & regex.IsMatch(acc.Owner.FullName.Trim())) ||
				(Criteria == "Паспортные данные" & regex.IsMatch(acc.Owner.PassportData))
				).ToList();


				if (found.Count > 0) {

					dataGridView1.Rows.Clear();
					Bank.FulfilDGV(dataGridView1, found);
				}

				else
				{
					MessageBox.Show("Указанная информация не найдена");
				}
			}

			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка поиска запроса: {ex.Message}");
			}
		}
	}
}
