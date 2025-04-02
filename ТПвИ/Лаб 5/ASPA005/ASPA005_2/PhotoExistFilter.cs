using DAL004;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO;

public class PhotoExistFilter : IEndpointFilter
{
	public static IRepository repository; 

	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		var celebrity = context.GetArgument<Celebrity>(0);
		var result = await next(context);

		string? fileName = Path.GetFileName(celebrity.PhotoPath);
		if (!File.Exists(Path.Combine(repository.BasePath, fileName)))
		{
			context.HttpContext.Response.Headers.Append("X-Celebrity", $"Not Found={fileName}");
		}

		return result;
	}
}
