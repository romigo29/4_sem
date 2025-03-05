using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace Bank
{
	[Serializable]
	public class Owner
	{
		[RegularExpression(@"^[А-ЯЁA-Z][а-яёa-z]+\s[А-ЯЁA-Z][а-яёa-z]+\s[А-ЯЁA-Z][а-яёa-z]+\s*$", ErrorMessage = "Неверный формат ФИО")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Отсуствует Дата рождения")]
		public DateTime BirthDate { get; set; }

		[PassportValidation]
		public string PassportData { get; set; }

		public Owner(string fullName, DateTime birthDate, string passportData)
		{
			FullName = fullName;
			BirthDate = birthDate;
			PassportData = passportData;
		}
	}
}
