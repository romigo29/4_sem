using DAL_Celebrity_MSSQL;
using static ANC25_WEBAPI_DLL.Services.CelebritiesAPIExtensions;
using static ANC25_WEBAPI_DLL.Services.CelebrityAPI;
using static ANC25_WEBAPI_DLL.Services.MiddlewareErrorHandler;
using ANC25_WEBAPI_DLL.Services;
using Microsoft.AspNetCore.Builder;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.AddCelebritiesConfiguration();
		builder.AddCelebritiesServices();

		builder.Services.AddControllersWithViews();
		var app = builder.Build();
		app.UseHttpsRedirection();

		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
		}
		app.UseStaticFiles();

		app.UseASPErrorHandler();

		app.MapCelebrities();
		app.MapLifeevents();
		app.MapPhotoCelebrities();

		app.UseAuthorization();

		app.MapControllerRoute(
			name: "celebrity",
			pattern: "/0",
			defaults: new { controller = "Celebrities", Action = "NewHumanForm" });

		app.MapControllerRoute(
			name: "celebrity",
			pattern: "/{id:int:min(1)}",
			defaults: new { controller = "Celebrities", Action = "Human" });

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Celebrities}/{action=Index}/{id?}");

		app.Run();
	}
}