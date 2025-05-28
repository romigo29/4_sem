using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_FitnessClub.Data;
using WPF_FitnessClub.Models;

namespace WPF_FitnessClub
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
		private readonly DataAccess _dataAccess;
		private readonly List<Review> _itemReviews; // Сохраняем отзывы

		public ObservableCollection<Subscription> Collection => _collection;

		public DeleteSubscriptionCommand(ObservableCollection<Subscription> collection, Subscription item, MainWindow mainWindow = null)
		{
			_collection = collection;
			_item = item;
			_dataAccess = new DataAccess();
			_itemReviews = new List<Review>(item.Reviews ?? new List<Review>());
		}

		public DeleteSubscriptionCommand(ObservableCollection<Subscription> subscriptions, Subscription currentSubscription)
		{
			_collection = subscriptions;
			_item = currentSubscription;
			_dataAccess = new DataAccess();
			_itemReviews = new List<Review>(currentSubscription.Reviews ?? new List<Review>());
		}

		public void Execute()
		{
			_index = _collection.IndexOf(_item);
			if (_index >= 0)
			{
				// Удаляем из коллекции
				_collection.Remove(_item);
				
				// Удаляем из базы данных
				int subscriptionId = _item.Id;
				_dataAccess.DeleteSubscription(subscriptionId);
			}
		}

		public void Undo()
		{
			if (_index >= 0 && _index <= _collection.Count)
			{
				// Вставляем обратно в коллекцию
				_collection.Insert(_index, _item);
				
				// Добавляем обратно в базу данных
				int newId = _dataAccess.AddSubscription(_item);
				_item.Id = newId;
				
				// Восстанавливаем отзывы
				if (_itemReviews != null && _itemReviews.Count > 0)
				{
					foreach (var review in _itemReviews)
					{
						_dataAccess.AddReview(review, newId);
					}
					_item.Reviews = new List<Review>(_itemReviews);
				}
			}
			else
			{
				// Добавляем в коллекцию
				_collection.Add(_item);
				
				// Добавляем обратно в базу данных
				int newId = _dataAccess.AddSubscription(_item);
				_item.Id = newId;
				
				// Восстанавливаем отзывы
				if (_itemReviews != null && _itemReviews.Count > 0)
				{
					foreach (var review in _itemReviews)
					{
						_dataAccess.AddReview(review, newId);
					}
					_item.Reviews = new List<Review>(_itemReviews);
				}
			}
		}
	}

	public class AddSubscriptionCommand : IActionCommand
	{
		private readonly ObservableCollection<Subscription> _collection;
		private readonly Subscription _item;
		private readonly DataAccess _dataAccess;

		public ObservableCollection<Subscription> Collection => _collection;

		public AddSubscriptionCommand(ObservableCollection<Subscription> collection, Subscription item, MainWindow mainWindow = null)
		{
			_collection = collection;
			_item = item;
			_dataAccess = new DataAccess();
		}

		public void Execute()
		{
			if (!_collection.Contains(_item))
			{
				// Добавляем в базу данных
				int newId = _dataAccess.AddSubscription(_item);
				_item.Id = newId;
				
				// Добавляем в коллекцию
				_collection.Add(_item);
			}
		}

		public void Undo()
		{
			_collection.Remove(_item);
			
			// Удаляем из базы данных
			_dataAccess.DeleteSubscription(_item.Id);
		}
	}

	public class EditSubscriptionCommand : IActionCommand
	{
		private readonly ObservableCollection<Subscription> _collection;
		private readonly Subscription _newState;
		private readonly Subscription _originalState;
		private readonly DataAccess _dataAccess;

		private int _index;

		public ObservableCollection<Subscription> Collection => _collection;
		
		public EditSubscriptionCommand(ObservableCollection<Subscription> collection, Subscription newState, Subscription originalState)
		{
			_collection = collection;
			_newState = newState;
			_originalState = originalState;
			_dataAccess = new DataAccess();
		}

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
				
				// Обновляем в базе данных
				_dataAccess.UpdateSubscription(_newState.Id, _newState);
			}
		}

		public void Undo()
		{
			if (_index != -1 && _index < _collection.Count)
			{
				ApplyChanges(_collection[_index], _originalState);
				
				// Обновляем в базе данных
				// Сначала удаляем текущие отзывы, если отзывы изменились
				if (_newState.Reviews.Count != _originalState.Reviews.Count || 
				    !_newState.Reviews.SequenceEqual(_originalState.Reviews))
				{
					int subscriptionId = _newState.Id;
					
					// Удаляем все текущие отзывы
					foreach (var review in _newState.Reviews.ToList())
					{
						// Тут бы хорошо иметь ID отзыва, но будем удалять через базу данных
						// непосредственно для абонемента
					}
					
					// Удаляем из базы данных и добавляем обратно с оригинальными отзывами
					_dataAccess.DeleteSubscription(subscriptionId);
					int newId = _dataAccess.AddSubscription(_originalState);
					_originalState.Id = newId;
					
					// Добавляем оригинальные отзывы
					foreach (var review in _originalState.Reviews)
					{
						_dataAccess.AddReview(review, newId);
					}
					
					// Синхронизируем ID в текущей коллекции
					_collection[_index].Id = newId;
				}
				else
				{
					// Если отзывы не изменились, просто обновляем базовую информацию
					_dataAccess.UpdateSubscription(_originalState.Id, _originalState);
				}
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
			
			// Обновляем отзывы, если они отличаются
			if (!target.Reviews.SequenceEqual(source.Reviews))
			{
				target.Reviews = new List<Review>(source.Reviews);
				target.Rating = source.Rating;
			}
		}
	}

}
