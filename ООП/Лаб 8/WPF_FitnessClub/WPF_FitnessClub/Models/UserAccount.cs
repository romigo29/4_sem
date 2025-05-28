using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_FitnessClub.Models
{
	public class UserAccount : INotifyPropertyChanged
	{
		private User _user;
		private List<Subscription> _subscriptions;
		private List<Visit> _visitHistory;
		private DateTime _registrationDate;
		private bool _isActive;
		private string _profileImagePath;

		public User User
		{
			get => _user;
			set
			{
				if (_user != value)
				{
					_user = value;
					OnPropertyChanged();
				}
			}
		}

		public List<Subscription> Subscriptions
		{
			get => _subscriptions;
			set
			{
				if (_subscriptions != value)
				{
					_subscriptions = value;
					OnPropertyChanged();
				}
			}
		}

		public List<Visit> VisitHistory
		{
			get => _visitHistory;
			set
			{
				if (_visitHistory != value)
				{
					_visitHistory = value;
					OnPropertyChanged();
				}
			}
		}

		public DateTime RegistrationDate
		{
			get => _registrationDate;
			set
			{
				if (_registrationDate != value)
				{
					_registrationDate = value;
					OnPropertyChanged();
				}
			}
		}

		public bool IsActive
		{
			get => _isActive;
			set
			{
				if (_isActive != value)
				{
					_isActive = value;
					OnPropertyChanged();
				}
			}
		}

		public string ProfileImagePath
		{
			get => _profileImagePath;
			set
			{
				if (_profileImagePath != value)
				{
					_profileImagePath = value;
					OnPropertyChanged();
				}
			}
		}

		public UserAccount(User user)
		{
			User = user;
			Subscriptions = new List<Subscription>();
			VisitHistory = new List<Visit>();
			RegistrationDate = DateTime.Now;
			IsActive = true;
			ProfileImagePath = "/Images/default_profile.jpg";
		}

		public UserAccount()
		{
			Subscriptions = new List<Subscription>();
			VisitHistory = new List<Visit>();
			RegistrationDate = DateTime.Now;
			IsActive = true;
			ProfileImagePath = "/Images/default_profile.jpg";
		}

		public void AddSubscription(Subscription subscription)
		{
			Subscriptions.Add(subscription);
			OnPropertyChanged(nameof(Subscriptions));
		}

		public void AddVisit(Visit visit)
		{
			VisitHistory.Add(visit);
			OnPropertyChanged(nameof(VisitHistory));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class Visit
	{
		public int Id { get; set; }
		public DateTime VisitDate { get; set; }
		public TimeSpan Duration { get; set; }
		public string Activity { get; set; }
		public string TrainerName { get; set; }

		public Visit(DateTime visitDate, TimeSpan duration, string activity, string trainerName = null)
		{
			VisitDate = visitDate;
			Duration = duration;
			Activity = activity;
			TrainerName = trainerName;
		}
	}
}
