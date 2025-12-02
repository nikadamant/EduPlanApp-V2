using EduPlanApp;
using EduPlanApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);


try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();

        // 1. Виконати міграції для створення або оновлення схеми
        context.Database.Migrate();

        // 2. Заповнити дані методом
        DbSeeder.Seed(context);

        // Логування
        app.Logger.LogInformation("Database migration and seeding completed successfully.");
    }
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "An error occurred during database migration or seeding.");
}

app.Run();