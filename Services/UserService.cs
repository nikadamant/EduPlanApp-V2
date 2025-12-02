using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduPlanApp.Services
{
    // Клас-заглушка для сервісу користувачів. 
    // Він би зберігав дані користувача у БД, але для нам потрібна лише мінімальна реалізація.
    public class UserService : IUserService
    {
        // Обробка результату аутентифікації від Google
        public Task<ClaimsPrincipal> HandleExternalLogin(AuthenticateResult result)
        {
            if (!result.Succeeded)
            {
                return Task.FromResult<ClaimsPrincipal>(null);
            }

            var identity = new ClaimsIdentity(result.Principal.Claims, "Google");

            var claims = identity.Claims.ToList();

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                claims.Add(new Claim("CustomAppRole", "User"));
            }

            var appIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(appIdentity);

            // Можлива логіка логіка:
            // 1. Пошук користувача в БД за email.
            // 2. Якщо знайдено, оновити його токени/дані.
            // 3. Якщо не знайдено, створити нового користувача.

            return Task.FromResult(principal);
        }
    }
}