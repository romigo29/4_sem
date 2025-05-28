using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WPF_FitnessClub.Models;

namespace WPF_FitnessClub.Data.Repositories
{
    public class SubscriptionRepository : BaseRepository<Subscription>
    {
        public SubscriptionRepository(AppDbContext context) : base(context)
        {
        }

        public override List<Subscription> GetAll()
        {
            try
            {
                // Используем SQL-запрос для получения данных с корректными именами полей
                var context = _context as AppDbContext;
                var subscriptions = new List<Subscription>();
                
                if (context == null)
                {
                    System.Diagnostics.Debug.WriteLine("GetAll: Ошибка приведения контекста к AppDbContext");
                    return new List<Subscription>();
                }
                
                // Получаем базовые данные абонементов
                var subscriptionsData = _dbSet.AsNoTracking().ToList();
                
                System.Diagnostics.Debug.WriteLine($"GetAll: загружено {subscriptionsData.Count} абонементов");
                
                foreach (var sub in subscriptionsData)
                {
                    // Для каждого абонемента загружаем его отзывы
                    System.Diagnostics.Debug.WriteLine($"GetAll: загрузка отзывов для абонемента {sub.Name} (ID={sub.Id})");
                    
                    var query = context.Reviews
                        .AsNoTracking()
                        .Where(r => r.SubscriptionId == sub.Id);
                    
                    // Логируем SQL-запрос
                    var sql = query.ToString();
                    System.Diagnostics.Debug.WriteLine($"GetAll: SQL-запрос для загрузки отзывов: {sql}");
                    
                    var reviewsQuery = query.ToList();
                    
                    System.Diagnostics.Debug.WriteLine($"GetAll: загружено {reviewsQuery.Count} отзывов для абонемента ID={sub.Id}");
                        
                    var reviews = new List<Review>();
                    
                    foreach (var r in reviewsQuery)
                    {
                        // Убеждаемся, что отзыв действительно связан с этим абонементом
                        if (r.SubscriptionId != sub.Id)
                        {
                            System.Diagnostics.Debug.WriteLine($"GetAll: ВНИМАНИЕ! Отзыв ID={r.Id} имеет SubscriptionId={r.SubscriptionId}, отличный от запрошенного ID={sub.Id}");
                            continue;
                        }
                        
                        // Получаем свойство User напрямую
                        reviews.Add(new Review
                        {
                            Id = r.Id,
                            User = r.User, // Напрямую используем свойство User
                            Score = r.Score,
                            Comment = r.Comment,
                            SubscriptionId = sub.Id, // Явно устанавливаем правильный ID абонемента
                            CreatedDate = DateTime.Now // Ставим значение по умолчанию
                        });
                        
                        System.Diagnostics.Debug.WriteLine($"GetAll: Добавлен отзыв ID={r.Id} от пользователя '{r.User}' для абонемента {sub.Id}");
                    }
                    
                    // Копируем свойства абонемента и устанавливаем отзывы
                    var subscription = new Subscription
                    {
                        Id = sub.Id,
                        Name = sub.Name,
                        Price = sub.Price,
                        Description = sub.Description,
                        ImagePath = sub.ImagePath,
                        Duration = sub.Duration,
                        SubscriptionType = sub.SubscriptionType,
                        Reviews = reviews
                    };
                    
                    subscriptions.Add(subscription);
                    
                    System.Diagnostics.Debug.WriteLine($"GetAll: абонемент {subscription.Name} (ID={subscription.Id}) добавлен с {reviews.Count} отзывами");
                }
                
                return subscriptions;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при получении абонементов: {ex.Message}");
                return new List<Subscription>();
            }
        }

        public override Subscription GetById(int id)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"GetById: начало загрузки абонемента с ID={id}");
                
                // Используем прямые запросы к контексту для получения абонемента и связанных отзывов
                var context = _context as AppDbContext;
                
                if (context == null)
                {
                    System.Diagnostics.Debug.WriteLine("GetById: Ошибка приведения контекста к AppDbContext");
                    return null;
                }
                
                // Получаем данные абонемента
                var subscription = _dbSet
                    .AsNoTracking()
                    .FirstOrDefault(s => s.Id == id);
                    
                if (subscription == null)
                {
                    System.Diagnostics.Debug.WriteLine($"GetById: абонемент с ID={id} не найден");
                    return null;
                }
                
                System.Diagnostics.Debug.WriteLine($"GetById: абонемент {subscription.Name} (ID={subscription.Id}) загружен успешно");
                    
                // Копируем свойства абонемента в новый объект
                var result = new Subscription
                {
                    Id = subscription.Id,
                    Name = subscription.Name,
                    Price = subscription.Price,
                    Description = subscription.Description,
                    ImagePath = subscription.ImagePath,
                    Duration = subscription.Duration,
                    SubscriptionType = subscription.SubscriptionType
                };
                
                // Загружаем отзывы для абонемента по точному ID
                System.Diagnostics.Debug.WriteLine($"GetById: загрузка отзывов для абонемента ID={id}");
                
                var query = context.Reviews
                    .AsNoTracking()
                    .Where(r => r.SubscriptionId == id);
                
                
                var reviewsQuery = query.ToList();
                
                System.Diagnostics.Debug.WriteLine($"GetById: загружено {reviewsQuery.Count} отзывов для абонемента ID={id}");
                    
                var reviews = new List<Review>();
                
                foreach (var r in reviewsQuery)
                {
                    // Убеждаемся, что отзыв действительно связан с этим абонементом
                    if (r.SubscriptionId != id)
                    {
                        System.Diagnostics.Debug.WriteLine($"GetById: ВНИМАНИЕ! Отзыв ID={r.Id} имеет SubscriptionId={r.SubscriptionId}, отличный от запрошенного ID={id}");
                        continue;
                    }
                    
                    // Получаем свойство User напрямую (оно маппится на UserName в базе данных)
                    reviews.Add(new Review
                    {
                        Id = r.Id,
                        User = r.User, // Просто используем свойство User напрямую
                        Score = r.Score,
                        Comment = r.Comment,
                        SubscriptionId = id, // Явно устанавливаем правильный ID абонемента
                        CreatedDate = DateTime.Now // Ставим значение по умолчанию
                    });
                    
                    System.Diagnostics.Debug.WriteLine($"GetById: Добавлен отзыв ID={r.Id} от пользователя '{r.User}' для абонемента {id}");
                }
                
                // Устанавливаем отзывы для абонемента
                result.Reviews = reviews;
                
                System.Diagnostics.Debug.WriteLine($"GetById: абонемент {result.Name} (ID={result.Id}) возвращается с {reviews.Count} отзывами");
                
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при получении абонемента по ID: {ex.Message}");
                return null;
            }
        }
    }
} 