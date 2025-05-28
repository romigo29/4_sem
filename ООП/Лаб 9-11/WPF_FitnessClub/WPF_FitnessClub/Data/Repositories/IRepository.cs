using System;
using System.Collections.Generic;

namespace WPF_FitnessClub.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Получить все записи
        List<T> GetAll();
        
        // Получить запись по идентификатору
        T GetById(int id);
        
        // Создать новую запись
        void Create(T entity);
        
        // Обновить существующую запись
        void Update(T entity);
        
        // Удалить запись
        void Delete(T entity);
        
        // Удалить запись по идентификатору
        void DeleteById(int id);
        
        // Сохранить изменения
        void Save();
    }
} 