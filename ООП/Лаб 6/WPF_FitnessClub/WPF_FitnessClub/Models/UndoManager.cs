using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_FitnessClub.Models
{

	public interface IActionCommand
	{
		void Execute();
		void Undo();
	}

	public class UndoManager
	{
		private readonly Stack<IActionCommand> _undoStack = new Stack<IActionCommand>();
		private readonly Stack<IActionCommand> _redoStack = new Stack<IActionCommand>();
		private MainWindow _mainWindow;

		public UndoManager(MainWindow mainWindow = null)
		{
			_mainWindow = mainWindow;
		}

		public void ExecuteAction(IActionCommand command)
		{
			command.Execute();
			_undoStack.Push(command);
			_redoStack.Clear(); // При новом действии Redo становится недоступным
			
			// Если команда обновляет коллекцию подписок, обновляем UI
			UpdateUIIfNeeded(command);
		}

		public void Undo()
		{
			if (_undoStack.Count == 0) return;

			var command = _undoStack.Pop();
			command.Undo();
			_redoStack.Push(command);
			
			// Обновляем UI после отмены операции
			UpdateUIIfNeeded(command);
		}

		public void Redo()
		{
			if (_redoStack.Count == 0) return;

			var command = _redoStack.Pop();
			command.Execute();
			_undoStack.Push(command);
			
			UpdateUIIfNeeded(command);
		}

		private void UpdateUIIfNeeded(IActionCommand command)
		{
			if (_mainWindow == null) return;
			
			if (command is AddSubscriptionCommand addCmd)
			{
				var updatedList = new List<Subscription>(addCmd.Collection);
				_mainWindow.UpdateUIWithSubscriptions(updatedList);
			}
			else if (command is DeleteSubscriptionCommand deleteCmd)
			{
				var updatedList = new List<Subscription>(deleteCmd.Collection);
				_mainWindow.UpdateUIWithSubscriptions(updatedList);
			}
			else if (command is EditSubscriptionCommand editCmd)
			{
				var updatedList = new List<Subscription>(editCmd.Collection);
				_mainWindow.UpdateUIWithSubscriptions(updatedList);
			}
		}

		public bool CanUndo => _undoStack.Count > 0;

		public bool CanRedo => _redoStack.Count > 0;

		public void Clear()
		{
			_undoStack.Clear();
			_redoStack.Clear();
		}
	}

	public class DeleteSubscriptionCommand : IActionCommand
	{
		private readonly ObservableCollection<Subscription> _collection;
		private readonly Subscription _item;
		private int _index;

		public ObservableCollection<Subscription> Collection => _collection;

		public DeleteSubscriptionCommand(ObservableCollection<Subscription> collection, Subscription item, MainWindow mainWindow = null)
		{
			_collection = collection;
			_item = item;
		}

		public void Execute()
		{
			_index = _collection.IndexOf(_item);
			if (_index >= 0)
				_collection.Remove(_item);
		}

		public void Undo()
		{
			if (_index >= 0 && _index <= _collection.Count)
				_collection.Insert(_index, _item);
			else
				_collection.Add(_item);
		}
	}

	public class AddSubscriptionCommand : IActionCommand
	{
		private readonly ObservableCollection<Subscription> _collection;
		private readonly Subscription _item;

		public ObservableCollection<Subscription> Collection => _collection;

		public AddSubscriptionCommand(ObservableCollection<Subscription> collection, Subscription item, MainWindow mainWindow = null)
		{
			_collection = collection;
			_item = item;
		}

		public void Execute()
		{
			if (!_collection.Contains(_item))
				_collection.Add(_item);
		}

		public void Undo()
		{
			_collection.Remove(_item);
		}
	}

	public class EditSubscriptionCommand : IActionCommand
	{
		private readonly ObservableCollection<Subscription> _collection;
		private readonly Subscription _newState;
		private readonly Subscription _originalState;

		private int _index;

		public ObservableCollection<Subscription> Collection => _collection;

		public void Execute()
		{
			_index = -1;
			for (int i = 0; i < _collection.Count; i++)
			{
				if (_collection[i] == _newState)
				{
					_index = i;
					break;
				}
			}

			if (_index != -1)
			{
				ApplyChanges(_collection[_index], _newState);
			}
		}

		public void Undo()
		{
			if (_index != -1 && _index < _collection.Count)
			{
				ApplyChanges(_collection[_index], _originalState);
			}
		}

		private void ApplyChanges(Subscription target, Subscription source)
		{
			target.Name = source.Name;
			target.Price = source.Price;
			target.Description = source.Description;
			target.ImagePath = source.ImagePath;
			target.Duration = source.Duration;
			target.SubscriptionType = source.SubscriptionType;
		}
	}

}
