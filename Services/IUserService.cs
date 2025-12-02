using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace EduPlanApp.Services
{
    // Інтерфейс для керування користувачами (заглушка)
    public interface IUserService
    {
        // Можлиа логіка пошуку/створення користувача БД
        Task<ClaimsPrincipal> HandleExternalLogin(AuthenticateResult result);
    }
}