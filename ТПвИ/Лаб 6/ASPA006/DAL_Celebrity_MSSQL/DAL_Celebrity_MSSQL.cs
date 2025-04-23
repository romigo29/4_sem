using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace DAL_Celebrity_MSSQL
{
	public class DAL_Celebrity_MSSQL
	{
		public interface IRepository : DAL_Celebrity.IRepository<Celebrity, Lifeevent> { }

		public class Celebrity  // Знаменитость
		{
			public Celebrity() { this.FullName = string.Empty; this.Nationality = string.Empty; }

			public int id { get; set; }                         // Id Знаменитости
			public string FullName { get; set; }                // полное имя Знаменитости
			public string Nationality { get; set; }             // гражданство Знаменитости (2 символа ISO)
			public string? ReqPhotoPath { get; set; }           // request path Фотографии

			public virtual bool Update(Celebrity celebrity)     // вспомогательный метод
			{
				if (celebrity == null) return false;
				
				this.FullName = celebrity.FullName;
				this.Nationality = celebrity.Nationality;
				this.ReqPhotoPath = celebrity.ReqPhotoPath;
				
				return true;
			}
		}

		public class Lifeevent  // Событие в жизни знаменитости
		{
			public Lifeevent() { this.Description = string.Empty; }

			public int id { get; set; }                         // Id События
			public int CelebrityId { get; set; }                // Id Знаменитости
			public DateTime? Date { get; set; }                 // дата События
			public string Description { get; set; }             // описание События
			public string? ReqPhotoPath { get; set; }           // request path Фотографии

			public virtual bool Update(Lifeevent lifeevent)     // вспомогательный метод
			{
				if (lifeevent == null) return false;

				this.CelebrityId = lifeevent.CelebrityId;
				this.Date = lifeevent.Date;
				this.Description = lifeevent.Description;
				this.ReqPhotoPath = lifeevent.ReqPhotoPath;

				return true;
			}
		}


		public class Repository : IRepository
		{
			Context context;

			public Repository() { this.context = new Context(); }

			public Repository(string connectionString)
			{
				this.context = new Context(connectionString);
			}

			public static IRepository Create() { return new Repository(); }

			public static IRepository Create(string connectionString)
			{
				return new Repository(connectionString);
			}

			public List<Celebrity> GetAllCelebrities()
			{
				return this.context.Celebrities.ToList<Celebrity>();
			}

			public Celebrity? GetCelebrityById(int id)
			{
				try
				{
					return this.context.Celebrities.FirstOrDefault(c => c.id == id);
				}
				catch
				{
					return null;
				}
			}

			public bool AddCelebrity(Celebrity celebrity)
			{
				try
				{
					this.context.Celebrities.Add(celebrity);
					this.context.SaveChanges();
					return true;
				}
				catch
				{
					return false;
				}
			}

			public bool DelCelebrity(int id)
			{
				try
				{
					Celebrity? celebrity = this.context.Celebrities.FirstOrDefault(c => c.id == id);
					if (celebrity != null)
					{
						this.context.Celebrities.Remove(celebrity);
						this.context.SaveChanges();
						return true;
					}
					return false;
				}
				catch
				{
					return false;
				}
			}

			public bool UpdCelebrity(Celebrity celebrity)
			{
				try
				{
					Celebrity? existingCelebrity = this.context.Celebrities.Find(celebrity.id);
					if (existingCelebrity != null)
					{
						if (existingCelebrity.Update(celebrity))
						{
							this.context.SaveChanges();
							return true;
						}
					}
					return false;
				}
				catch
				{
					return false;
				}
			}

			public List<Lifeevent> GetAllLifeevents()
			{
				return this.context.LifeEvents.ToList<Lifeevent>();
			}

			public Lifeevent? GetLifeeventById(int id)
			{
				try
				{
					return this.context.LifeEvents.FirstOrDefault(l => l.CelebrityId == id);
				}
				catch
				{
					return null;
				}
			}

			public bool AddLifeevent(Lifeevent lifeevent)
			{
				try
				{
					this.context.LifeEvents.Add(lifeevent);
					this.context.SaveChanges();
					return true;
				}
				catch
				{
					return false;
				}
			}

			public bool DelLifeevent(int id)
			{
				try
				{
					Lifeevent? lifeevent = this.context.LifeEvents.FirstOrDefault(l => l.id == id);
					if (lifeevent != null)
					{
						this.context.LifeEvents.Remove(lifeevent);
						this.context.SaveChanges();
						return true;
					}
					return false;
				}
				catch
				{
					return false;
				}
			}

			public bool UpdLifeevent(Lifeevent lifeevent)
			{
				try
				{
					Lifeevent? existingLifeevent = this.context.LifeEvents.FirstOrDefault(l => l.id == lifeevent.id);
					if (existingLifeevent != null)
					{
						if (existingLifeevent.Update(lifeevent))
						{
							this.context.SaveChanges();
							return true;
						}
					}
					return false;
				}
				catch
				{
					return false;
				}
			}

			public List<Lifeevent> GetLifeeventsByCelebrityId(int celebrityId)
			{
				try
				{
					return this.context.LifeEvents.Where(l => l.CelebrityId == celebrityId).ToList();
				}
				catch
				{
					return new List<Lifeevent>();
				}
			}

			public Celebrity? GetCelebrityByLifeeventId(int lifeeventId)
			{
				try
				{
					Lifeevent? lifeevent = this.context.LifeEvents.FirstOrDefault(l => l.id == lifeeventId);
					if (lifeevent != null)
					{
						return this.context.Celebrities.FirstOrDefault(c => c.id == lifeevent.CelebrityId);
					}
					return null;
				}
				catch
				{
					return null;
				}
			}

			public int GetCelebrityIdByName(string name)
			{
				try
				{
					Celebrity? celebrity = this.context.Celebrities.FirstOrDefault(c => c.FullName.Contains(name));
					return celebrity != null ? celebrity.id : -1;
				}
				catch
				{
					return -1;
				}
			}

			public void Dispose()
			{
				// освобождение ресурсов
				this.context.Dispose();
			}
		}


		public class Context : DbContext
		{
			public string? ConnectionString { get; private set; } = null;

			public Context(string connstring) : base()
			{
				this.ConnectionString = connstring;
				// this.Database.EnsureDeleted();
				// this.Database.EnsureCreated();
			}

			public Context() : base()
			{
				// this.Database.EnsureDeleted();
				// this.Database.EnsureCreated();
			}

			public DbSet<Celebrity> Celebrities { get; set; }
			public DbSet<Lifeevent> LifeEvents { get; set; }

			protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			{
				if (this.ConnectionString == null) this.ConnectionString = @"Data source=172.16.193.88; Initial Catalog=LES01;" +
						@"TrustServerCertificate=True;User Id = smw60; Password=21625";
				optionsBuilder.UseSqlServer(this.ConnectionString);
			}

			protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
				modelBuilder.Entity<Celebrity>().ToTable("Celebrities").HasKey(p => p.id);
				modelBuilder.Entity<Celebrity>().Property(p => p.id).IsRequired();
				modelBuilder.Entity<Celebrity>().Property(p => p.FullName).IsRequired().HasMaxLength(50);
				modelBuilder.Entity<Celebrity>().Property(p => p.Nationality).IsRequired().HasMaxLength(2);
				modelBuilder.Entity<Celebrity>().Property(p => p.ReqPhotoPath).HasMaxLength(200);

				modelBuilder.Entity<Lifeevent>().ToTable("Lifeevents").HasKey(p => p.id);
				modelBuilder.Entity<Lifeevent>().ToTable("Lifeevents");
				modelBuilder.Entity<Lifeevent>().Property(p => p.id).IsRequired();
				modelBuilder.Entity<Lifeevent>().ToTable("Lifeevents").HasOne<Celebrity>().WithMany().HasForeignKey(p => p.CelebrityId);
				modelBuilder.Entity<Lifeevent>().Property(p => p.CelebrityId).IsRequired();
				modelBuilder.Entity<Lifeevent>().Property(p => p.Date);
				modelBuilder.Entity<Lifeevent>().Property(p => p.Description).HasMaxLength(256);
				modelBuilder.Entity<Lifeevent>().Property(p => p.ReqPhotoPath).HasMaxLength(256);

				base.OnModelCreating(modelBuilder);
			}
		}


	}
}
