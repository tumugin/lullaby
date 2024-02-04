using Lullaby;

#pragma warning disable CA1852

var builder = WebApplication.CreateBuilder(args);
builder.AddApplicationServices();

using var app = builder.Build();
app.UseLullabyWebApplication()
    .Run();
