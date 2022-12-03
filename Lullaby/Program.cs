using Lullaby;
using Lullaby.Data;
using Lullaby.Services.Event;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LullabyContext>(options =>
    DatabaseConfig.CreateDbContextOptions(dbConnectionString, options)
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<GetEventsByGroupKeyService, GetEventsByGroupKeyService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.UseAuthorization();

app.Run();
