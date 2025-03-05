using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank
{

	public class Account
	{

		public static int AccountAmount;
		public static string LastAction = "Нет действий";

		[RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Неверный формат номера счета")]
		public string AccountNumber { get; set; }

		public Owner Owner { get; set; }

		[Required(ErrorMessage = "Отсуствует тип счета")]
		public string AccountType { get; set; }
		public double Balance { get; set; }
		public DateTime OpeningDate { get; set; }
		public bool IsSMS { get; set; }
		public bool IsInternetBanking { get; set; }

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

		public Account() { }
	}

}
