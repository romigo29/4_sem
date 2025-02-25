using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
	public class Account
	{
		public string Number { get; set; }
		public Owner Owner { get; set; }
		public string AccountType { get; set; }
		public double Balance { get; set; }
		public DateTime OpeningDate { get; set; }
		public bool IsSMS { get; set; }
		public bool IsInternetBanking { get; set; }

		public Account(string number, Owner owner, string accountType, double balance, DateTime openingDate, bool isSMS, bool isInternetBanking)
		{
			Number = number;
			Owner = owner;
			AccountType = accountType;
			Balance = balance;
			OpeningDate = openingDate;
			IsSMS = isSMS;
			IsInternetBanking = isInternetBanking;
		}
	}

}
