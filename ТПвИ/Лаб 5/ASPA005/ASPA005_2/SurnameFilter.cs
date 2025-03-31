using DAL004;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

public class SurnameFilter : IEndpointFilter
{
	public static IRepository repository; // Репозиторий для проверки дубликатов

	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		var celebrity = context.GetArgument<Celebrity>(0);
		if (string.IsNullOrWhiteSpace(celebrity.Surname) || celebrity.Surname.Length < 2)
		{
			return Results.BadRequest("Surname cannot be empty or too short");
		}

		if (repository.getCelebritiesBySurname(celebrity.Surname).Length > 0)
		{
			return Results.BadRequest("Surname already exists");
		}

		return await next(context);
	}
}
