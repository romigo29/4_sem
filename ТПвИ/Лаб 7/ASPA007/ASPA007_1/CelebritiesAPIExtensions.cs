using Microsoft.Extensions.Options;
using static DAL_Celebrity_MSSQL.DAL_Celebrity_MSSQL;

namespace ASPA007_1
{
	public static class CelebritiesAPIExtensions
	{
		public static IServiceCollection AddCelebritiesConfiguration(this WebApplicationBuilder builder, string celebrityjson = "Celebrities.config.json")
		{
			builder.Configuration.AddJsonFile(celebrityjson);
			return builder.Services.Configure<CelebritiesConfig>(builder.Configuration.GetSection("Celebrities"));
		}

		public static IServiceCollection AddCelebritiesServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<IRepository, IRepository>((IServiceProvider p) =>
			{
				CelebritiesConfig config = p.GetRequiredService<IOptions<CelebritiesConfig>>().Value;
				return new Repository(config.ConnectionString);
			});

			builder.Services.AddSingleton<CelebrityTitles>((p) => new CelebrityTitles());

			return builder.Services;
		}

		public class CelebrityTitles
		{
			public string Head { get; } = "Celebrities Dictionary Internet Service";
			public string Title { get; } = "Celebrities";
			public string Copyright { get; } = "@Copyright BSTU";
		}

	}
}
