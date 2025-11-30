using EduPlanApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduPlanApp.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
