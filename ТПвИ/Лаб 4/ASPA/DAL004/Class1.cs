using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace DAL004
{
	public record Celebrity(int Id, string Firstname, string Surname, string PhotoPath);
	public interface IRepository : IDisposable
	{
		string BasePath { get; }
		Celebrity[] getAllCelebrities();
		Celebrity? getCelebrityById(int id);
		Celebrity[] getCelebritiesBySurname(string Surname);
		string? getPhotoPathById(int id);

		int? addCelebrity(Celebrity celebrity);  // добавить знаменитость, =Id новой знаменитости
		bool delCelebrityById(int id);  // удалить знаменитость по Id, =true – успех
		int? updCelebrityById(int id, Celebrity celebrity);  // изменить знаменитость по Id, =Id – новый Id = успех
		int SaveChanges();  // сохранить изменения в JSON, =количество изменений
	}
	public class Repository : IRepository
	{
		public static string JSONFileName = "Celebrities.json";
		private List<Celebrity> _celebrities;
		private string _jsonFilePath;

		public string BasePath { get; }

		public Repository(string basePath)
		{
			BasePath = Path.Combine(Directory.GetCurrentDirectory(), basePath);
			_jsonFilePath = Path.Combine(BasePath, JSONFileName);
			_celebrities = new List<Celebrity>();

			if (!Directory.Exists(BasePath))
			{
				Directory.CreateDirectory(BasePath);
			}
			if (!File.Exists(_jsonFilePath))
			{
				File.WriteAllText(_jsonFilePath, "[]");
			}

			LoadData();
		}

		public static Repository Create(string dir) => new Repository(dir);

		private void LoadData()
		{
			string jsonContent = File.ReadAllText(_jsonFilePath);
			_celebrities = JsonSerializer.Deserialize<List<Celebrity>>(jsonContent) ?? new List<Celebrity>();
		}

		public Celebrity[] getAllCelebrities()
		{
			return _celebrities.ToArray();
		}

		public Celebrity? getCelebrityById(int id)
		{

			var index = _celebrities.FindIndex(c => c.Id == id);
			if (index == -1)
			{
				return null;
			}

			return _celebrities[index];
		}

		public Celebrity[] getCelebritiesBySurname(string Surname)
		{
			return _celebrities.Where(c => c.Surname.Equals(Surname, StringComparison.OrdinalIgnoreCase)).ToArray();
		}

		public string? getPhotoPathById(int id)
		{
			var celebrity = getCelebrityById(id);
			return celebrity?.PhotoPath;
		}

		public void Dispose() { }

		public int? addCelebrity(Celebrity celebrity)
		{
			////task 1-21
			//var SearchPhotoPath = Path.Combine(BasePath, celebrity.PhotoPath.Split('/').Last());
			//if (!File.Exists(SearchPhotoPath))
			//{
			//	throw new InvalidPhotoPathException($"Could not find file '{SearchPhotoPath}'.");
			//}

			if (_celebrities.Any(c =>
			c.Id == celebrity.Id ||
			c.Firstname == celebrity.Firstname ||
			c.Surname == celebrity.Surname ||
			c.PhotoPath == celebrity.PhotoPath))
			{
				return null;
			}

			int NewId = _celebrities.Count > 0 ? _celebrities.Max(c => c.Id) + 1 : 1;
			celebrity = new Celebrity(NewId, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
			_celebrities.Add(celebrity);
			SaveChanges();

			return NewId;

		}
		public bool delCelebrityById(int id)
		{
			if (!_celebrities.Any(c => c.Id == id))
				return false;

			_celebrities.RemoveAll(c => c.Id == id);
			SaveChanges();

			return true;
		}

		public int? updCelebrityById(int id, Celebrity celebrity)
		{
			var index = _celebrities.FindIndex(c => c.Id == id);
			if (index == -1)
			{
				return null;
			}

			_celebrities[index] = new Celebrity(id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
			SaveChanges();

			return id;
		}

		public int SaveChanges()
		{
			string json = JsonSerializer.Serialize(_celebrities);
			File.WriteAllText(_jsonFilePath, json);
			return _celebrities.Count;
			
		}
	}
	public class FoundByIdException : Exception { public FoundByIdException(string message) : base($"Found by Id: {message}") { } }
	public class SaveException : Exception { public SaveException(string message) : base($"SaveChanges error: {message}") { } }
	public class AddCelebrityException : Exception { public AddCelebrityException(string message) : base($"AddCelebrityException error: {message}") { } }
	public class InvalidPhotoPathException : Exception { public InvalidPhotoPathException(string message) : base(message) { }}

	public class DelByIdException : Exception { public DelByIdException(string message) : base($"Delete by Id:{message}") { } }

	public class UpdException : Exception { public UpdException(string message) : base($"Upd by{message}") { } }

	public class AbsourdException : Exception { public AbsourdException(string message) : base($"Value: {message}") { } }

}

