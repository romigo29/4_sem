using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_FitnessClub
{
	public class Subscription
	{

		string Name;
		double Price;
		string Description;
		string ImagePath;
		string Duration;

		public Subscription(string name, double price, string description, string path, string duration)
		{
			Name = name;
			Price = price;
			Description = description;
			ImagePath = path;
			Duration = duration;
		}

	}
}
