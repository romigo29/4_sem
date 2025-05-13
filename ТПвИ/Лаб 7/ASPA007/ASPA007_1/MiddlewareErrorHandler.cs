using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ASPA007_1
{
	public static class MiddlewareErrorHandler
	{
		public static WebApplication UseASPErrorHandler(this WebApplication app)
		{
			app.Map("/Celebrities/Error", (HttpContext ctx) =>
			{
				Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
				IResult rc = Results.Problem(detail: "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);
				return rc;
			});
			
			app.Map("/Lifeevents/Error", (HttpContext ctx) =>
			{
				Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
				IResult rc = Results.Problem(detail: "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);
				return rc;
			});
			
			return app;
		}
	}
}
