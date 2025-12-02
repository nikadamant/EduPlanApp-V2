using EduPlanApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduPlanApp.Controllers
{
    [Authorize]
    public class ReferenceController : Controller
    {
        private readonly AppDbContext _context;

        public ReferenceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            ViewData["Title"] = "Довідник Категорій";
            var categories = await _context.Categories.ToListAsync();
            return View("~/Views/Reference/CategoryList.cshtml", categories);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDetails(int id)
        {
            ViewData["Title"] = "Деталі Категорії";
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View("~/Views/Reference/CategoryDetails.cshtml", category);
        }

        [HttpGet]
        public async Task<IActionResult> PriorityList()
        {
            ViewData["Title"] = "Довідник Пріоритетів";
            var priorities = await _context.Priorities.ToListAsync();
            return View(priorities);
        }

        [HttpGet]
        public async Task<IActionResult> PriorityDetails(int id)
        {
            ViewData["Title"] = "Деталі Пріоритету";
            var priority = await _context.Priorities.FirstOrDefaultAsync(p => p.Id == id);

            if (priority == null)
            {
                return NotFound();
            }

            return View(priority);
        }
    }
}