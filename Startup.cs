using Microsoft.AspNetCore.Authorization;

namespace EduPlanApp
{
    public class Startup
    {
        private const string GoogleClientId = "158597371582-e8ihcgl7mfcnin484oreu8mbuko681r1.apps.googleusercontent.com";
        private const string GoogleClientSecret = "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";

        public void ConfigureServices(IServiceCollection services)
        {
            // 1. Налаштування Identity/Authentication 
            services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options => { options.LoginPath = "/Account/Login"; })

                // ДОДАЄМО ПРОВАЙДЕРА GOOGLE (OAuth2)
                .AddGoogle(options =>
                {
                    options.ClientId = GoogleClientId;
                    options.ClientSecret = GoogleClientSecret;
                    options.CallbackPath = "/signin-google";
                });

            // 2. Додавання MVC сервісів
            services.AddControllersWithViews();

            // 3. Налаштування політики авторизації
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
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
