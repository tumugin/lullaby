using Lullaby.Admin;

var builder = WebApplication.CreateBuilder(args);
builder.AddApplicationServices();

using var app = builder.Build();
app.UseWebApplication()
    .Run();
