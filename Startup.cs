using Microsoft.AspNetCore.Authorization;

namespace EduPlanApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // 1. Налаштування Identity/Authentication (Обов'язково для OAuth2)
            // НАСТУПНИЙ КРОК: Тут буде додано AddAuthentication/AddOpenIdConnect для OAuth2
            // Наразі додаємо заглушку для локальної авторизації, щоб AuthZ працювала.
            services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options => { options.LoginPath = "/Account/Login"; });

            // 2. Додавання MVC сервісів
            services.AddControllersWithViews();

            // 3. Налаштування політики авторизації
            services.AddAuthorization(options =>
            {
                /*options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();*/
            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            // Middleware для маршрутизації та авторизації
            app.UseStaticFiles(); // Для обслуговування CSS, JS, зображень
            app.UseRouting();

            app.UseAuthentication(); // Використовувати аутентифікацію (ПЕРЕД UseAuthorization)
            app.UseAuthorization();  // Використовувати авторизацію

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Welcome}/{id?}"); // Початкова сторінка: Home/Welcome
            });
        }
    }


}
