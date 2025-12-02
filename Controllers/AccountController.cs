using EduPlanApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduPlanApp.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Тут мала б бути логіка реєстрації користувача у БД
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // --- ДІЇ ДЛЯ GOOGLE OAUTH2 ---

        /// <summary>
        /// Початок зовнішньої аутентифікації (перенаправлення на Google).
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            // Налаштування для перенаправлення до провайдера
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            // Виклик провайдера (наприклад, "Google")
            return Challenge(properties, provider);
        }

        /// <summary>
        /// Обробка відповіді від Google.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = "/Account/Profile", string? remoteError = null)
        {
            if (remoteError != null)
            {
                // Обробка помилки, якщо Google відхилив запит (наприклад, користувач скасував)
                ModelState.AddModelError(string.Empty, $"Помилка зовнішнього провайдера: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await HttpContext.AuthenticateAsync("Google");
            if (info?.Principal == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);
            var externalId = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);


            // 1. Створення claims для нашого локального Cookie-авторизації
            var claims = new List<Claim>
            {
                // Використовуємо зовнішній ID як NameIdentifier для нашої локальної системи
                new Claim(ClaimTypes.NameIdentifier, externalId!),
                new Claim(ClaimTypes.Email, email!),
                new Claim(ClaimTypes.Name, name!),
                new Claim("AuthenticationType", "Google")
            };

            var appIdentity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(appIdentity);

            // 2. Вхід користувача в нашу систему за допомогою Cookies
            await HttpContext.SignInAsync("Cookies", principal);

            // Перенаправлення за вказаним URL
            return LocalRedirect(returnUrl ?? "/Account/Profile");
        }

        /// <summary>
        /// Вихід користувача.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");

            return RedirectToAction(nameof(Login));
        }


        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}