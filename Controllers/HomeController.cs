using EduPlanApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduPlanApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
