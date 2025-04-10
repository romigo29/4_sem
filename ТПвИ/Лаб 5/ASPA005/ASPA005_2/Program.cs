using DAL004;
using Microsoft.AspNetCore.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using FluentValidation;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		var app = builder.Build();


		Repository.JSONFileName = "Celebrities.json";
		using (IRepository repository = Repository.Create("Celebrities"))
		{

			RouteGroupBuilder api = app.MapGroup("/Celebrities");

			app.UseExceptionHandler("/Celebrities/Error");

			api.MapGet("/", () => repository.getAllCelebrities());  // ASPA03

			api.MapGet("/{id:int}", (int id) =>
			{
				Celebrity? celebrity = repository.getCelebrityById(id);
				if (celebrity == null) throw new FoundByIdException($"Celebrity Id = {id}");
				return celebrity;
			});


			SurnameFilter.repository = PhotoExistFilter.repository =
				FirstnameFilter.repository = IdFilter.repository = repository;

			api.MapPost("/", (Celebrity celebrity) =>
			{
				int? id = repository.addCelebrity(celebrity);
				if (id == null) throw new AddCelebrityException("/Celebrities error, id == null");
				if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
				return new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
			})
			.AddEndpointFilter<SurnameFilter>()  // Проверка фамилии
			.AddEndpointFilter<PhotoExistFilter>(); // Проверка фото

			api.MapDelete("/{id:int}", (int id) =>
			{
				if (!repository.delCelebrityById(id)) throw new DelByIdException($"DELETE /Celebrities error, ID = {id}");
				return Results.Ok($"Celebrity with Id = {id} deleted");
			})
			.AddEndpointFilter<IdFilter>();

			api.MapPut("/{id:int}", (int id, Celebrity celebrity) =>
			{
				int? newId = repository.updCelebrityById(id, celebrity);

				return new Celebrity((int)newId, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
			})
			.AddEndpointFilter<IdFilter>();

			app.MapFallback((HttpContext ctx) => Results.NotFound(new { error = $"path {ctx.Request.Path} not supported" }));

			app.Map("/Celebrities/Error", (HttpContext ctx) =>
			{
				Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
				IResult rc = Results.Problem(detail: "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);
				if (ex != null)
				{

					if (ex is FoundByIdException) rc = Results.NotFound(ex.Message);  
					if (ex is BadHttpRequestException) rc = Results.BadRequest(ex.Message);  
					if (ex is SaveException)
						rc = Results.Problem(title: "ASPA004/SaveChanges", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
					if (ex is AddCelebrityException)
						rc = Results.Problem(title: "ASPA004/addCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
					if (ex is InvalidPhotoPathException)
						rc = Results.Problem(title: "ASPA004", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
					if (ex is DelByIdException)
						rc = Results.NotFound(ex.Message);
					if (ex is UpdException)
						rc = Results.Problem(ex.Message);
					if (ex is AbsourdException)
						rc = Results.Conflict(ex.Message);
				}
				return rc;
			});

			app.Run();
		}

	}
}

