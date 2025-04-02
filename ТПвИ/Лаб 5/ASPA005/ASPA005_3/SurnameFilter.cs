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
			return Results.BadRequest("Surname is wrong");
		}

		if (repository.getCelebritiesBySurname(celebrity.Surname).Length > 0)
		{
			return Results.BadRequest("Surname is doubled");
		}

		return await next(context);
	}
}
