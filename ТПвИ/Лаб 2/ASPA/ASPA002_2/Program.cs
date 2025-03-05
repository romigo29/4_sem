using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder();

var app = builder.Build();

app.UseDefaultFiles(new DefaultFilesOptions
{
	DefaultFileNames = new List<string> { "Neumann.html" }
});

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
	RequestPath = "/static" 
});


app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Picture")),
	RequestPath = "/images"
});


app.Run();
