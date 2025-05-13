using Microsoft.Extensions.Options;
using static DAL_Celebrity_MSSQL.DAL_Celebrity_MSSQL;

namespace ASPA007_1
{
	public static class CelebrityAPI
	{

		public static RouteHandlerBuilder MapCelebrities(this IEndpointRouteBuilder routeBuilder, string prefix = "/api/Celebrities")
		{
			// ---------- ЗНАМЕНИТОСТИ (Celebrities) -----------------------------------
			var celebrities = routeBuilder.MapGroup("/api/Celebrities");

			celebrities.MapGet("/", (IRepository repo) => repo.GetAllCelebrities());

			celebrities.MapGet("/{id:int:min(1)}", (IRepository repo, int id) =>
			{
				var celebrity = repo.GetCelebrityById(id);
				if (celebrity == null)
					return Results.NotFound();
				return Results.Ok(celebrity);
			});

			celebrities.MapGet("/Lifeevents/{id:int:min(1)}", (IRepository repo, int id) =>
			{

				var celebrity = repo.GetLifeeventById(id);
				if (celebrity == null)
					return Results.NotFound();
				return Results.Ok(celebrity);
			});

			celebrities.MapDelete("/{id:int:min(1)}", (IRepository repo, int id) =>
			{
				var celebrity = repo.GetCelebrityById(id);
				if (celebrity == null)
					return Results.NotFound(new
					{
						type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
						title = "Not Found",
						status = 404,
						detail = $"404002:Celebrity Id = {id}",
						instance = "ANC25"
					});

				if (repo.DelCelebrity(id))
					return Results.Ok(celebrity);
				return Results.Problem();
			});

			celebrities.MapPost("/", (IRepository repo, Celebrity celebrity) =>
			{
				if (celebrity == null)
					return Results.Problem();

				repo.AddCelebrity(celebrity);
				return Results.Ok(celebrity);
			});

			celebrities.MapPut("/{id:int:min(1)}", (IRepository repo, int id, Celebrity celebrity) =>
			{
				if (celebrity == null)
					return Results.Problem();

				if (celebrity.id != id)
				{
					celebrity.id = id;
				}

				if (repo.UpdCelebrity(celebrity))
					return Results.Ok(celebrity);
				else
					return Results.NotFound();
			});

			string GetContentTypeByExtension(string extension)
			{
				return extension.ToLower() switch
				{
					".jpg" => "image/jpeg",
					".jpeg" => "image/jpeg",
					".png" => "image/png",
					_ => "application/octet-stream",
				};
			}


			return celebrities.MapGet("/photo/{fname}", async (IOptions<CelebritiesConfig> iconfig, HttpContext context, string fname) =>
			{
				CelebritiesConfig config = iconfig.Value;
				string path = Path.Combine(config.PhotoPath, fname);

				Console.WriteLine($"Запрос на фото: {path}");

				if (!File.Exists(path))
				{
					Console.WriteLine("Файл не найден");
					return Results.NotFound();
				}

				try
				{
					var bytes = await File.ReadAllBytesAsync(path);
					string contentType = GetContentTypeByExtension(Path.GetExtension(path));
					return Results.File(bytes, contentType);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
					return Results.Problem($"Ошибка при чтении файла: {ex.Message}");
				}
			});


		}

		public static RouteHandlerBuilder MapPhotoCelebrities(this IEndpointRouteBuilder routebuilder, string? prefix = "/Photos")
		{
			if (string.IsNullOrEmpty(prefix)) prefix = routebuilder.ServiceProvider.GetRequiredService<IOptions<CelebritiesConfig>>().Value.PhotosRequestPath;
			return routebuilder.MapGet($"{prefix}/{{fname}}", async (IOptions<CelebritiesConfig> iconfig, HttpContext context, string fname) =>
			{
				CelebritiesConfig config = iconfig.Value;
				string filePath = Path.Combine(config.PhotoPath, fname);
				FileStream file = File.OpenRead(filePath);
				BinaryReader sr = new BinaryReader(file);
				BinaryWriter sw = new BinaryWriter(context.Response.BodyWriter.AsStream());
				int n = 0; byte[] buffer = new byte[2048];
				context.Response.ContentType = "image/jpeg";
				context.Response.StatusCode = StatusCodes.Status200OK;
				while ((n = await sr.BaseStream.ReadAsync(buffer, 0, 2048)) > 0) await sw.BaseStream.WriteAsync(buffer, 0, n);
				sr.Close(); sw.Close();
			});
		}

		public static RouteHandlerBuilder MapLifeevents(this IEndpointRouteBuilder routeBuilder, string prefix = "/api/Lifeevents")
		{

			var lifeevents = routeBuilder.MapGroup("/api/Lifeevents");

			lifeevents.MapGet("/", (IRepository repo) => repo.GetAllLifeevents());

			lifeevents.MapGet("/{id:int:min(1)}", (IRepository repo, int id) =>
			{
				var lifeevent = repo.GetLifeeventById(id);
				if (lifeevent == null)
					return Results.NotFound($"Событие с ID {id} не найдено");

				return Results.Ok(lifeevent);
			});

			lifeevents.MapGet("/Celebrities/{id:int:min(1)}", (IRepository repo, int id) =>
			{
				var lifeevents = repo.GetLifeeventsByCelebrityId(id);
				if (lifeevents == null || !lifeevents.Any())
					return Results.NotFound($"События для знаменитости с ID {id} не найдены");

				return Results.Ok(lifeevents);
			});

			lifeevents.MapDelete("/{id:int:min(1)}", (IRepository repo, int id) =>
			{
				var lifeevent = repo.GetLifeeventById(id);
				if (lifeevent == null)
					return Results.NotFound(new
					{
						type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
						title = "Not Found",
						status = 404,
						detail = $"404002:Lifeevent Id = {id}",
						instance = "ANC25"
					});

				if (repo.DelLifeevent(id))
					return Results.Ok(lifeevent);
				return Results.Problem();
			});

			lifeevents.MapPost("/", (IRepository repo, Lifeevent lifeevent) =>
			{
				if (lifeevent == null)
					return Results.Problem();

				repo.AddLifeevent(lifeevent);
				return Results.Ok(lifeevent);
			});

			return lifeevents.MapPut("/{id:int:min(1)}", (IRepository repo, Lifeevent lifeevent) =>
			{
				if (lifeevent == null)
					return Results.NotFound(lifeevent);

				repo.UpdLifeevent(lifeevent);
				return Results.Ok("Событие успешно добавлено");
			});

		}



	}
}
