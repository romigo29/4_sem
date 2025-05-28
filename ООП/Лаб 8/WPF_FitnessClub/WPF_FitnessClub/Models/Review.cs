using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_FitnessClub
{
	public class Review : IEquatable<Review>
	{
		string user;
		int score;
		string comment;

		public Review(string user, int score, string comment)
		{
			this.user = user;
			this.score = score;
			this.comment = comment;
		}

		public string User
		{
			get => user;
			set
			{
				if (user != value)
				{
					user = value;
					OnPropertyChanged("User");
				}
			}
		}

		public int Score
		{
			get => score;
			set
			{
				if (score != value)
				{
					score = value;
					OnPropertyChanged("Score");
				}
			}
		}

		public string Comment
		{
			get => comment;
			set
			{
				if (comment != value)
				{
					comment = value;
					OnPropertyChanged("Comment");
				}
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
		
		// Реализация интерфейса IEquatable<Review> для корректного сравнения отзывов
		public bool Equals(Review other)
		{
			if (other == null)
				return false;

			return this.User == other.User &&
				   this.Score == other.Score &&
				   this.Comment == other.Comment;
		}

		// Переопределение метода Equals для поддержки сравнения с объектами
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			return Equals((Review)obj);
		}

		// Переопределение метода GetHashCode для поддержки хеширования
		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + (User != null ? User.GetHashCode() : 0);
			hash = hash * 23 + Score.GetHashCode();
			hash = hash * 23 + (Comment != null ? Comment.GetHashCode() : 0);
			return hash;
		}
	}
}
