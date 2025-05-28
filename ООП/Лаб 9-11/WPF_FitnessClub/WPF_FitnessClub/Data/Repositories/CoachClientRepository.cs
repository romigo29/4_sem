using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using WPF_FitnessClub.Models;

namespace WPF_FitnessClub.Data.Repositories
{
    public class CoachClientRepository : BaseRepository<CoachClient>
    {
        public CoachClientRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Получить всех клиентов тренера
        /// </summary>
        public List<User> GetCoachClients(int coachId)
        {
            try
            {
                var context = _context as AppDbContext;
                if (context == null)
                {
                    return new List<User>();
                }

                // Используем JOIN для получения всех клиентов тренера одним запросом
                var clients = context.Users
                    .AsNoTracking()
                    .Join(
                        context.CoachClients.Where(cc => cc.CoachId == coachId),
                        user => user.Id,
                        coachClient => coachClient.ClientId,
                        (user, coachClient) => user
                    )
                    .ToList();

                return clients;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при получении клиентов тренера: {ex.Message}");
                return new List<User>();
            }
        }

        /// <summary>
        /// Проверить, является ли клиент клиентом тренера
        /// </summary>
        public bool IsClientAssignedToCoach(int clientId, int coachId)
        {
            try
            {
                return _dbSet
                    .AsNoTracking()
                    .Any(cc => cc.ClientId == clientId && cc.CoachId == coachId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при проверке связи клиент-тренер: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Добавить клиента тренеру
        /// </summary>
        public bool AssignClientToCoach(int clientId, int coachId)
        {
            System.Diagnostics.Debug.WriteLine($"Начало метода AssignClientToCoach: clientId={clientId}, coachId={coachId}");
            
            try
            {
                // Проверяем входные данные
                if (clientId <= 0 || coachId <= 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Неверные данные: clientId={clientId}, coachId={coachId}");
                    return false;
                }
                
                // Проверяем, что такой связи еще нет
                if (IsClientAssignedToCoach(clientId, coachId))
                {
                    System.Diagnostics.Debug.WriteLine($"Связь уже существует: clientId={clientId}, coachId={coachId}");
                    return true; // Связь уже существует
                }
                
                // Проверяем, что клиент и тренер существуют
                var context = _context as AppDbContext;
                if (context != null)
                {
                    var clientExists = context.Users.Any(u => u.Id == clientId);
                    var coachExists = context.Users.Any(u => u.Id == coachId);
                    
                    if (!clientExists || !coachExists)
                    {
                        System.Diagnostics.Debug.WriteLine($"Клиент или тренер не существует: clientExists={clientExists}, coachExists={coachExists}");
                        return false;
                    }
                }

                System.Diagnostics.Debug.WriteLine("Создание нового объекта CoachClient");
                var coachClient = new CoachClient
                {
                    // Не задаем Id, пусть его создаст база данных или просто не используем
                    CoachId = coachId,
                    ClientId = clientId,
                    AssignedDate = DateTime.Now
                };

                System.Diagnostics.Debug.WriteLine("Добавление записи в DbSet");
                _dbSet.Add(coachClient);
                
                try
                {
                    System.Diagnostics.Debug.WriteLine("Сохранение изменений в базу данных");
                    _context.SaveChanges();
                    
                    System.Diagnostics.Debug.WriteLine($"Клиент успешно добавлен к тренеру: clientId={clientId}, coachId={coachId}");
                    return true;
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
                {
                    System.Diagnostics.Debug.WriteLine($"DbUpdateException: {dbEx.Message}");
                    if (dbEx.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");
                        if (dbEx.InnerException.InnerException != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Inner Inner Exception: {dbEx.InnerException.InnerException.Message}");
                        }
                    }
                    
                    // Попробуем SQL запрос напрямую, если имя колонки Id не соответствует
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Попытка добавления записи через прямой SQL запрос");
                        var ctx = _context as AppDbContext;
                        if (ctx != null)
                        {
                            string sql = $"INSERT INTO CoachClients (CoachId, ClientId, AssignedDate) VALUES ({coachId}, {clientId}, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";
                            System.Diagnostics.Debug.WriteLine($"SQL запрос: {sql}");
                            ctx.Database.ExecuteSqlCommand(sql);
                            System.Diagnostics.Debug.WriteLine("SQL запрос выполнен успешно");
                            return true;
                        }
                    }
                    catch (Exception sqlEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Ошибка SQL запроса: {sqlEx.Message}");
                    }
                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Исключение при добавлении клиента тренеру: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
                
                return false;
            }
        }

        /// <summary>
        /// Удалить клиента у тренера
        /// </summary>
        public bool RemoveClientFromCoach(int clientId, int coachId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Удаление клиента у тренера: clientId={clientId}, coachId={coachId}");

                // Проверяем существование связи
                if (!IsClientAssignedToCoach(clientId, coachId))
                {
                    System.Diagnostics.Debug.WriteLine($"Связь не существует: clientId={clientId}, coachId={coachId}");
                    return true; // Нечего удалять
                }
                
                // Получаем запись для удаления
                System.Diagnostics.Debug.WriteLine("Поиск записи для удаления");
                var coachClient = _dbSet
                    .FirstOrDefault(cc => cc.ClientId == clientId && cc.CoachId == coachId);

                if (coachClient == null)
                {
                    System.Diagnostics.Debug.WriteLine("Запись не найдена");
                    return true; // Нечего удалять
                }

                // Сначала удаляем связанные планы питания
                try
                {
                    var context = _context as AppDbContext;
                    if (context != null)
                    {
                        // Удаляем планы питания
                        var nutritionPlansToDelete = context.NutritionPlans
                            .Where(np => np.ClientId == clientId && np.CoachId == coachId)
                            .ToList();
                            
                        if (nutritionPlansToDelete.Any())
                        {
                            System.Diagnostics.Debug.WriteLine($"Удаление {nutritionPlansToDelete.Count} планов питания для клиента {clientId}");
                            context.NutritionPlans.RemoveRange(nutritionPlansToDelete);
                        }

                        // Удаляем тренировочные планы
                        var workoutPlansToDelete = context.WorkoutPlans
                            .Where(wp => wp.ClientId == clientId && wp.CoachId == coachId)
                            .ToList();
                            
                        if (workoutPlansToDelete.Any())
                        {
                            System.Diagnostics.Debug.WriteLine($"Удаление {workoutPlansToDelete.Count} тренировочных планов для клиента {clientId}");
                            context.WorkoutPlans.RemoveRange(workoutPlansToDelete);
                        }

                        // Сохраняем изменения перед удалением связи
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка при удалении связанных данных: {ex.Message}");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine("Удаление записи из DbSet");
                _dbSet.Remove(coachClient);
                
                try
                {
                    System.Diagnostics.Debug.WriteLine("Сохранение изменений в базу данных");
                    _context.SaveChanges();
                    
                    System.Diagnostics.Debug.WriteLine($"Клиент успешно удален у тренера: clientId={clientId}, coachId={coachId}");
                    return true;
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
                {
                    System.Diagnostics.Debug.WriteLine($"DbUpdateException: {dbEx.Message}");
                    if (dbEx.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");
                        if (dbEx.InnerException.InnerException != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Inner Inner Exception: {dbEx.InnerException.InnerException.Message}");
                        }
                    }
                    
                    // Попробуем SQL запрос напрямую
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Попытка удаления записи через прямой SQL запрос");
                        var ctx = _context as AppDbContext;
                        if (ctx != null)
                        {
                            string sql = $"DELETE FROM CoachClients WHERE ClientId = {clientId} AND CoachId = {coachId}";
                            System.Diagnostics.Debug.WriteLine($"SQL запрос: {sql}");
                            ctx.Database.ExecuteSqlCommand(sql);
                            System.Diagnostics.Debug.WriteLine("SQL запрос выполнен успешно");
                            return true;
                        }
                    }
                    catch (Exception sqlEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Ошибка SQL запроса: {sqlEx.Message}");
                    }
                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Исключение при удалении клиента у тренера: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
                
                return false;
            }
        }

        /// <summary>
        /// Получить список ID клиентов для тренера
        /// </summary>
        /// <param name="coachId">ID тренера</param>
        /// <returns>Список ID клиентов</returns>
        public List<int> GetCoachClientIds(int coachId)
        {
            try
            {
                // Используем Entity Framework для получения списка ID клиентов
                return _dbSet
                    .AsNoTracking()
                    .Where(cc => cc.CoachId == coachId)
                    .Select(cc => cc.ClientId)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при получении ID клиентов тренера: {ex.Message}");
                return new List<int>(); // Возвращаем пустой список в случае ошибки
            }
        }

        /// <summary>
        /// Получить список клиентов, у которых нет тренера (не связаны с тренерами в таблице CoachClients)
        /// </summary>
        /// <returns>Список клиентов без тренера</returns>
        public List<User> GetClientsWithoutCoach()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Начало метода GetClientsWithoutCoach");
                
                var context = _context as AppDbContext;
                if (context == null)
                {
                    System.Diagnostics.Debug.WriteLine("Контекст равен null");
                    return new List<User>();
                }

                // Более эффективный метод - используем LEFT JOIN для проверки отсутствия клиента в таблице CoachClient
                var clientsWithoutCoach = context.Users
                    .AsNoTracking()
                    .Where(u => u.Role == UserRole.Client)
                    .GroupJoin(
                        context.CoachClients,
                        user => user.Id,
                        cc => cc.ClientId,
                        (user, coachClients) => new { User = user, CoachClients = coachClients }
                    )
                    .SelectMany(
                        x => x.CoachClients.DefaultIfEmpty(),
                        (x, cc) => new { User = x.User, CoachClient = cc }
                    )
                    .Where(x => x.CoachClient == null)
                    .Select(x => x.User)
                    .ToList();
                
                System.Diagnostics.Debug.WriteLine($"Найдено клиентов без тренеров: {clientsWithoutCoach.Count}");
                
                return clientsWithoutCoach;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при получении клиентов без тренера: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
                return new List<User>();
            }
        }
        
        /// <summary>
        /// Получить даты добавления клиентов к тренеру
        /// </summary>
        /// <param name="coachId">ID тренера</param>
        /// <returns>Словарь, где ключ - ID клиента, значение - дата добавления</returns>
        public Dictionary<int, DateTime> GetClientAssignmentDates(int coachId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Получение дат добавления клиентов для тренера ID: {coachId}");
                
                var context = _context as AppDbContext;
                if (context == null)
                {
                    System.Diagnostics.Debug.WriteLine("Контекст равен null");
                    return new Dictionary<int, DateTime>();
                }
                
                // Получаем все связи тренер-клиент для данного тренера
                var coachClients = _dbSet
                    .AsNoTracking()
                    .Where(cc => cc.CoachId == coachId)
                    .ToList();
                
                // Создаем словарь с датами
                var datesDictionary = new Dictionary<int, DateTime>();
                
                foreach (var relation in coachClients)
                {
                    datesDictionary[relation.ClientId] = relation.AssignedDate;
                }
                
                System.Diagnostics.Debug.WriteLine($"Получено {datesDictionary.Count} записей с датами добавления");
                
                return datesDictionary;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при получении дат добавления клиентов: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
                return new Dictionary<int, DateTime>();
            }
        }
    }
} 