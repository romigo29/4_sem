﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPF_FitnessClub.Models
{
	public class Subscription : INotifyPropertyChanged
	{

		int id;
		string name;
		double price;
		string description;
		string imagePath;
		string duration;
		string subscriptionType;
		List<Review> reviews;
		double rating;


		#region Свойства
		public int Id
		{
			get => id;
			set
			{
				if (id != value)
				{
					id = value;
					OnPropertyChanged("Id");
				}
			}
		}

		public string Name
		{
			get => name;
			set
			{
				if (name != value)
				{
					name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		public double Price
		{
			get => price;
			set
			{
				if (price != value)
				{
					price = value;
					OnPropertyChanged("Price");
				}
			}
		}

		public string Description
		{
			get => description;
			set
			{
				if (description != value)
				{
					description = value;
					OnPropertyChanged("Description");
				}
			}
		}

		public string ImagePath
		{
			get => imagePath;
			set
			{
				if (imagePath != value)
				{
					imagePath = value;
					OnPropertyChanged("imagePath");
				}
			}
		}

		public string Duration
		{
			get => duration;
			set
			{
				if (duration != value)	
				{
					duration = value;
					OnPropertyChanged("Duration");
				}
			}
		}	

		public string SubscriptionType
		{
			get => subscriptionType;
			set
			{	
				if (subscriptionType != value)
				{
					subscriptionType = value;
					OnPropertyChanged("SubscriptionType");
				}
			}
		}	
		
		public List<Review> Reviews
		{
			get => reviews;
			set
			{
				if (reviews != value)
				{
					reviews = value;
					OnPropertyChanged("Reviews");
				}
			}
		}

		public double Rating
		{
			get => rating;
			set
			{
				if (rating != value)
				{
					rating = value;
					OnPropertyChanged("Rating");

				}
			}
		}

		#endregion
		public Subscription()
		{
			this.name = "";
			this.price = 0;
			this.description = "";
			this.imagePath = "";
			this.duration = "";
			this.subscriptionType = "";
			this.reviews = new List<Review>();
			this.rating = 0.0;
		}
		public Subscription(string name, double price, string description, string path, string duration, string subscriptionType, List<Review> reviews)
		{
			this.name = name;
			this.price = price;
			this.description = description;
			this.imagePath = path;
			this.duration = duration;
			this.subscriptionType = subscriptionType;
			this.reviews = reviews;
			this.rating = CalculateRating();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}

		public double CalculateRating()
		{

			if (Reviews == null || Reviews.Count == 0) return 0.0;

			double _rating = Reviews.Where(r => r.Score > 0).ToList().Average(r => r.Score);
			
			return double.Parse(_rating.ToString("F1"));
		}

	}

	
}
