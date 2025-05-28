using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WPF_FitnessClub.Models
{
	public class User
	{

		string fullName;
		string email;
		string login;
		string password;
		UserRole role;

		public User(string fullName, string email, string login, string password, UserRole role)
		{
			FullName = fullName;
			Email = email;
			Login = login;
			Password = password;
			Role = role;

		}

		public string FullName
		{
			get => fullName;
			set
			{
				if (fullName != value)
				{
					fullName = value;
					OnPropertyChanged("FullName");
				}
			}
		}

		public string Email
		{
			get => email;
			set
			{
				if (email != value)
				{
					email = value;
					OnPropertyChanged("Email");
				}
			}
		}

		public string Login
		{
			get => login;
			set
			{
				if (login != value)
				{
					login = value;
					OnPropertyChanged("Login");
				}
			}
		}

		public string Password
		{
			get => password;
			set
			{
				if (password != value)
				{
					password = value;
					OnPropertyChanged("Password");
				}
			}
		}

		public UserRole Role
		{
			get => role;
			set
			{
				if (role != value)
				{
					role = value;
					OnPropertyChanged("Role");
				}
			}
		}



		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}


	}

	public enum UserRole
	{
		Client = 1,
		Coach = 2,
		Admin = 3
	}
}
