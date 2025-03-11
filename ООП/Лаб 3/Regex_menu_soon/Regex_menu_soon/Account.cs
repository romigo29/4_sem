using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank
{
	public class Account
	{

		public static int AccountAmount = 0;
		public static string LastAction = "Нет действий";

		[Required]
		[RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Неверный формат номера счета")]
		public string AccountNumber { get; set; }

		public Owner Owner { get; set; }

		[Required(ErrorMessage = "Отсуствует тип счета")]
		public string AccountType { get; set; }
		public double Balance { get; set; }
		public DateTime OpeningDate { get; set; }
		public bool IsSMS { get; set; }
		public bool IsInternetBanking { get; set; }

		public Account() { }

		public Account(string number, Owner owner, string accountType, double balance, DateTime openingDate, bool isSMS, bool isInternetBanking)
		{
			AccountNumber = number;
			Owner = owner;
			AccountType = accountType;
			Balance = balance;
			OpeningDate = openingDate;
			IsSMS = isSMS;
			IsInternetBanking = isInternetBanking;
		}

		public static void ShowAccountsAmount() => MessageBox.Show($"Количество клиентов: {AccountAmount}");

		public static void IncreaseClients() => AccountAmount++;
		public static void ClearClients() => AccountAmount = 0;

		public static void ChangeLastAction(string message) => LastAction = message;

	}

}
