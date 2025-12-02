using System;
using System.Threading;
using System.Threading.Tasks;
using EduPlanApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EduPlanApp.Services
{
    public class DbServiceInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DbServiceInitializer> _logger;

        public DbServiceInitializer(IServiceProvider serviceProvider, ILogger<DbServiceInitializer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Запуск ініціалізації бази даних...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    var dbType = context.Database.ProviderName;

                    if (dbType!.Contains("InMemory"))
                    {
                        // Для In-Memory база даних створюється автоматично при першому доступі
                        _logger.LogInformation("Використовується In-Memory DB. Міграції пропущені.");
                    }
                    else if (dbType.Contains("Sqlite"))
                    {
                        // Для Sqlite створюємо БД, якщо вона не існує
                        await context.Database.EnsureCreatedAsync(cancellationToken);
                        _logger.LogInformation("Використовується SQLite. База даних гарантовано створена.");
                    }
                    else
                    {
                        // Для MS-SQL та Postgres застосовуємо міграції
                        _logger.LogInformation($"Застосування міграцій для БД...");
                        await context.Database.MigrateAsync(cancellationToken);
                        _logger.LogInformation("Міграції успішно застосовані.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Помилка під час міграції або ініціалізації бази даних.");
                    // Тут може бути логіка повтору
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}