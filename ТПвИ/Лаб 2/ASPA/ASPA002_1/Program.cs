var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseWelcomePage("/aspnetcore");
app.MapGet("/aspnetcore", () => "Мое первое ASP");

app.Run();
