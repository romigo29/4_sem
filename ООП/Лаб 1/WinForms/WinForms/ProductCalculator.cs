using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
	public partial class ProductCalculator : Form
	{
		List<Product> products = new List<Product>();

		public class Product
		{
			public readonly double Markup = 15;
			public string Name { get; set; }
			public double Amount { get; set; }

			public double Value { get; set; }

			public double DailyUsage { get; set; }

			public Product(string name, double amount, double value, double dailyUsage)
			{
				Name = name;
				Amount = amount;
				Value = value;
				DailyUsage = dailyUsage;
			}
		}


		public ProductCalculator()
		{
			InitializeComponent();
		}

		private void ProductCalculator_Load(object sender, EventArgs e)
		{
		
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{

			products.Clear();
			
			foreach( DataGridViewRow row in dataGridView1.Rows)
			{
				if (row.IsNewRow) continue;

				string name = row.Cells[0].Value.ToString().Trim();
				double amount = 0, value = 0, dailyUsage = 0;

				try
				{
					if (string.IsNullOrEmpty(name))	throw new Exception("Название товара не может быть пустым");

					if (!double.TryParse(row.Cells[1].Value.ToString(), out amount) || amount <= 0) throw new Exception("Введено некорректное значение количества товара");

					if (!double.TryParse(row.Cells[2].Value.ToString(), out value) || value <= 0) throw new Exception("Введено некорректное значение стоимости товара");

					if (!double.TryParse(row.Cells[3].Value.ToString(), out dailyUsage) || dailyUsage <= 0) throw new Exception("Введено некорректное значение ежеденевного потребления товара");

				}

				catch (Exception ex) 
				{
					MessageBox.Show($"Ошибка: {ex.Message}");
				}

				products.Add(new Product(name, amount, value, dailyUsage));
			}

		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
		}
		private void buttonValuePerWeight_Click(object sender, EventArgs e)
		{
			Output.Clear();
			string output = "Стоимость товар за килограмм: \n";

			foreach (Product product in products)
			{
			
				output += $"{product.Name} \t - \t";
				output += $"{(product.Value * 1000) / product.Amount}р\n";
			}

			this.Output.Text = output;
		}

		private void buttonCostPrice_Click(object sender, EventArgs e)
		{
			Output.Clear();

			string output = "Себестоимость товаров: \n";

			foreach (Product product in products)
			{

				output += $"{product.Name} \t - \t";
				output += $"{product.Value / (1 + product.Markup / 100)}р\n";
			}

			this.Output.Text = output;

		}

		private void ButtonMonthUsages_Click(object sender, EventArgs e)
		{
			Output.Clear();

			const double nMonths = 30;
			string output = "Расходов товаров товаров: \n";

			foreach (Product product in products)
			{

				output += $"{product.Name} \t - \t";
				output += $"{((product.DailyUsage * nMonths) / product.Amount) * product.Value}р/месяц\n";
			}

			this.Output.Text = output;
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}
	}
}
