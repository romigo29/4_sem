using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Bank
{
	public class Owner
	{
		public string FullName { get; set; }
		public DateTime BirthDate { get; set; }
		public string PassportData { get; set; }

		public Owner(string fullName, DateTime birthDate, string passportData)
		{
			FullName = fullName;
			BirthDate = birthDate;
			PassportData = passportData;
		}
	}
}
