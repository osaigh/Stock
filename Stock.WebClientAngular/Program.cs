var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// In development the Angular dev server (http://localhost:44902) serves the client and
// Microsoft.AspNetCore.SpaProxy starts it and redirects the browser to it.
// When published, the built client is served from wwwroot.
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapFallbackToFile("index.html");

app.Run();
