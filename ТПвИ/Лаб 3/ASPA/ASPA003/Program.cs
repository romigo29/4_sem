using DAL003;
using Microsoft.Extensions.FileProviders;
using System;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.Services.AddDirectoryBrowser();
		var app = builder.Build();

		var fileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Celebrities"));
		var requestPath = "/Celebrities/download";

		app.UseStaticFiles(new StaticFileOptions
		{
			FileProvider = fileProvider,

			/*//task 4
			RequestPath = "/Photo"*/

			//task 5
			RequestPath = requestPath,

			//task 5 - file download
			OnPrepareResponse = ctx =>
			{
				ctx.Context.Response.Headers.Append("Content-Disposition", "attachment");
			}
		});

		app.UseDirectoryBrowser(new DirectoryBrowserOptions
		{
			FileProvider = fileProvider,
			RequestPath = requestPath
		});

		Repository.JSONFileName = "Celebrities.json";
		using (IRepository repository = Repository.Create("Celebrities"))
		{
			app.MapGet("/Celebrities",
				() => repository.getAllCelebrities());

			app.MapGet("/Celebrities/{id:int}",
				(int id) => repository.getCelebrityById(id));

			app.MapGet("/Celebrities/BySurname/{surname}",
				(string surname) => repository.getCelebritiesBySurname(surname));

			app.MapGet("/Celebrities/PhotoPathById/{id:int}",
				(int id) => repository.getPhotoPathById(id));
		}

		app.Run();
	}
}