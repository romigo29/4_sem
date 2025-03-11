using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace DAL003
{
    public record Celebrity(int Id, string Firstname, string Surname, string PhotoPath);
    public interface IRepository : IDisposable
    {
        string BasePath { get; }
        Celebrity[] getAllCelebrities();
        Celebrity? getCelebrityById(int id);
        Celebrity[] getCelebritiesBySurname(string Surname);
        string? getPhotoPathById(int id);
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
            return _celebrities.FirstOrDefault(c => c.Id == id);
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
    }
}

