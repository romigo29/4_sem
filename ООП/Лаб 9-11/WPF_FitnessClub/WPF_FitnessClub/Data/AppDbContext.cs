using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WPF_FitnessClub.Models;
using WPF_FitnessClub.DataBase;

namespace WPF_FitnessClub.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext() : base("FitnessClubConnectionString")
		{
			// Отключаем LazyLoading, чтобы управлять загрузкой связанных сущностей самостоятельно
			Configuration.LazyLoadingEnabled = false;
			
			// Отключаем автоматическое обнаружение изменений
			Configuration.AutoDetectChangesEnabled = true;
			
			// Отключаем инициализатор базы данных, чтобы управлять миграциями вручную
			Database.SetInitializer<AppDbContext>(null);
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<NutritionPlan> NutritionPlans { get; set; }
		public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
		public DbSet<CoachClient> CoachClients { get; set; }
		public DbSet<UserSubscription> UserSubscriptions { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// Отключаем каскадное удаление по умолчанию
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

			// Настройка для свойства Role в User
			modelBuilder.Entity<User>()
				.Property(u => u.Role)
				.HasColumnName("Role")
				.IsRequired()
				.HasColumnType("int");

			// Настройка связи 1:M между Subscription и Review
			modelBuilder.Entity<Subscription>()
				.HasMany(s => s.Reviews)
				.WithRequired(r => r.Subscription)
				.HasForeignKey(r => r.SubscriptionId);

			// Настройка связей для UserSubscription
			modelBuilder.Entity<UserSubscription>()
				.HasRequired(us => us.User)
				.WithMany()
				.HasForeignKey(us => us.UserId)
				.WillCascadeOnDelete(false);
				
			modelBuilder.Entity<UserSubscription>()
				.HasRequired(us => us.Subscription)
				.WithMany()
				.HasForeignKey(us => us.SubscriptionId)
				.WillCascadeOnDelete(false);

			// Настройка связей для NutritionPlan
			modelBuilder.Entity<NutritionPlan>()
				.HasRequired(np => np.Client)
				.WithMany()
				.HasForeignKey(np => np.ClientId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<NutritionPlan>()
				.HasRequired(np => np.Coach)
				.WithMany()
				.HasForeignKey(np => np.CoachId)
				.WillCascadeOnDelete(false);

			// Настройка связей для WorkoutPlan
			modelBuilder.Entity<WorkoutPlan>()
				.HasRequired(wp => wp.Client)
				.WithMany()
				.HasForeignKey(wp => wp.ClientId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<WorkoutPlan>()
				.HasRequired(wp => wp.Coach)
				.WithMany()
				.HasForeignKey(wp => wp.CoachId)
				.WillCascadeOnDelete(false);

			// Настройка связей для CoachClient
			modelBuilder.Entity<CoachClient>()
				.HasRequired(cc => cc.Coach)
				.WithMany()
				.HasForeignKey(cc => cc.CoachId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<CoachClient>()
				.HasRequired(cc => cc.Client)
				.WithMany()
				.HasForeignKey(cc => cc.ClientId)
				.WillCascadeOnDelete(false);

			base.OnModelCreating(modelBuilder);
		}
	}
}
