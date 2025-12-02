using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using EduPlanApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using EduPlanApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace EduPlanApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private const string GoogleClientId = "158597371582-e8ihcgl7mfcnin484oreu8mbuko681r1.apps.googleusercontent.com";
        private const string GoogleClientSecret = "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";

        public void ConfigureServices(IServiceCollection services)
        {
            var dbType = Configuration.GetValue<string>("DatabaseType");
            var connectionString = "";

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<AppDbContext>(options =>
            {
                switch (dbType)
                {
                    case "MsSql":
                        connectionString = Configuration.GetConnectionString("MsSqlConnection");
                        options.UseSqlServer(connectionString);
                        break;
                    case "Postgres":
                        connectionString = Configuration.GetConnectionString("PostgresConnection");
                        options.UseNpgsql(connectionString);
                        break;
                    case "Sqlite":
                        connectionString = Configuration.GetConnectionString("SqliteConnection");
                        options.UseSqlite(connectionString);
                        break;
                    case "InMemory":
                        options.UseInMemoryDatabase("EduPlanAppInMemoryDB");
                        break;
                    default:
                        throw new ArgumentException($"Невідомий тип бази даних: {dbType}");
                }
            });

            // Додаємо DbInitializer як хостовану службу
            services.AddHostedService<DbServiceInitializer>();
            services.AddScoped<IUserService, UserService>();


            // 1. Налаштування Identity/Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Google";
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            })
            .AddGoogle(options =>
            {
                options.ClientId = GoogleClientId;
                options.ClientSecret = GoogleClientSecret;
                options.CallbackPath = "/signin-google";
                options.Scope.Add("profile");
            });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Welcome}/{id?}");
            });
        }
    }
}