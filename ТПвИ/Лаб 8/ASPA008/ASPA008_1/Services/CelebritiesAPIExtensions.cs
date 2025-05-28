using Microsoft.Extensions.Options;
using System.Text.Json;
using static ASPA008_1.Services.CelebritiesAPIExtensions.CountryCodes;
using static DAL_Celebrity_MSSQL.DAL_Celebrity_MSSQL;

namespace ASPA008_1.Services
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

			builder.Services.AddScoped<IRepository, Repository>((IServiceProvider p) =>
			{

				CelebritiesConfig config = p.GetRequiredService<IOptions<CelebritiesConfig>>().Value;

				return new Repository(config.ConnectionString);

			});

			builder.Services.AddSingleton<CelebrityTitles>((p) => new CelebrityTitles());

			builder.Services.AddSingleton<CountryCodes>((p) => new CountryCodes(

			p.GetRequiredService<IOptions<CelebritiesConfig>>().Value.ISO3166alpha2Path

			));

			return builder.Services;

		}

		public class CountryCodes : List<ISOCountryCodes>
		{

			public record ISOCountryCodes(string code, string countryLabel);

			public CountryCodes(string jsoncountrycodespath) : base()

			{

				if (File.Exists(jsoncountrycodespath))

				{

					FileStream fs = new FileStream(jsoncountrycodespath, FileMode.OpenOrCreate, FileAccess.Read);

					List<ISOCountryCodes>? cc = JsonSerializer.DeserializeAsync<List<ISOCountryCodes>?>(fs).Result;

					if (cc != null) this.AddRange(cc);

				}
			}

			public class CelebrityTitles
			{
				public string Head { get; } = "Celebrities Dictionary Internet Service";
				public string Title { get; } = "Celebrities";
				public string Copyright { get; } = "@Copyright BSTU";
			}

		}
	}
}