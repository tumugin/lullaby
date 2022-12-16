using Lullaby;
using Lullaby.Data;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                         ?? throw new InvalidOperationException("DB ConnectionString must not be null.");
builder.Services.AddDbContext<LullabyContext>(options =>
    DatabaseConfig.CreateDbContextOptions(dbConnectionString, options)
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
DiConfig.BuildDi(builder);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Quartz
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    if (builder.Environment.EnvironmentName != "Testing")
    {
        q.UsePersistentStore(store =>
        {
            store.UseJsonSerializer();
            store.UseProperties = true;
            store.UseClustering();
            store.UseMySqlConnector((c) =>
            {
                c.ConnectionString = dbConnectionString;
            });
        });
    }
});
builder.Services.AddQuartzHostedService(q =>
{
    q.WaitForJobsToComplete = true;
});

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
