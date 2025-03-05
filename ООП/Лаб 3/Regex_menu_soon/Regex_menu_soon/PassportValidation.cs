using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bank
{
	public class PassportValidation : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value != null)
			{
				string passport = value.ToString();
				string passportRegex = @"^[A-Z]{2}\d{7}$";
				if (Regex.IsMatch(passport, passportRegex)) return true;
				else this.ErrorMessage = "Паспорт должен быть записан в виде MP1234567";
			}

			return false;
		}
	}
}
