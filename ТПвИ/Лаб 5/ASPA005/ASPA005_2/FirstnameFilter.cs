using DAL004;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO;

public class FirstnameFilter : IEndpointFilter
{
	public static IRepository repository;

	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		var celebrity = context.GetArgument<Celebrity>(0);
		if (string.IsNullOrWhiteSpace(celebrity.Firstname) || celebrity.Firstname.Length < 2)
		{
			return Results.Conflict("Firstname is wrong");
		}

		if (repository.getCelebritiesBySurname(celebrity.Firstname).Length > 0)
		{
			return Results.Conflict("Firstname is doubled");
		}

		return await next(context);
	}
}
