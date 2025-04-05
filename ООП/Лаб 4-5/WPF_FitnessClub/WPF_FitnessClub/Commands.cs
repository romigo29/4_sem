using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_FitnessClub
{
	internal class Commands
	{

		public ICommand EditCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }
		public ICommand DeleteCommand { get; private set; }
		public ICommand DeleteReviewCommand { get; private set; }
		public ICommand CategoryChangedCommand { get; private set; }

		public class RelayCommand : ICommand
		{
			private readonly Action<object> _execute;
			private readonly Func<object, bool> _canExecute;

			public event EventHandler CanExecuteChanged
			{
				add { CommandManager.RequerySuggested += value; }
				remove { CommandManager.RequerySuggested -= value; }
			}

			public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
			{
				_execute = execute ?? throw new ArgumentNullException(nameof(execute));
				_canExecute = canExecute;
			}

			public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
			public void Execute(object parameter) => _execute(parameter);
		}
	}
}
