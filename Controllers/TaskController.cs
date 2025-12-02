using EduPlanApp.Data;
using EduPlanApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduPlanApp.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        private async Task LoadReferenceData(TaskInputModel model)
        {
            model.AvailableCategories = await _context.Categories
                .ToDictionaryAsync(c => c.Id, c => c.Name);

            model.AvailablePriorities = await _context.Priorities
                .ToDictionaryAsync(p => p.Id, p => p.Name);
        }

        private IQueryable<TaskViewModel> GetBaseTaskQuery()
        {
            return _context.TaskItems
                .Include(t => t.Category) // JOIN 1: Категорія
                .Include(t => t.Priority) // JOIN 2: Пріоритет
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description ?? string.Empty,
                    CreatedDate = t.CreatedDate,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    CategoryName = t.Category.Name,
                    PriorityName = t.Priority.Name
                })
                .OrderBy(t => t.DueDate); // Сортування за найближчим терміном
        }

        [HttpGet]
        public async Task<IActionResult> TaskList()
        {
            ViewData["Title"] = "Список Навчальних Завдань";
            var tasks = await GetBaseTaskQuery().ToListAsync();
            return View("~/Views/Task/TaskList.cshtml", tasks);
        }

        [HttpGet]
        public async Task<IActionResult> TaskDetails(int id)
        {
            ViewData["Title"] = "Деталі Завдання";
            var task = await GetBaseTaskQuery()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View("~/Views/Task/TaskDetails.cshtml", task);
        }


        [HttpGet]
        public async Task<IActionResult> TaskCreator()
        {
            ViewData["Title"] = "Створення Завдання";
            var model = new TaskInputModel();
            await LoadReferenceData(model);
            return View("~/Views/Task/TaskCreator.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaskCreator(TaskInputModel model)
        {
            ModelState.Remove("AvailableCategories");
            ModelState.Remove("AvailablePriorities");

            if (ModelState.IsValid)
            {
                var taskItem = new TaskItem
                {
                    Title = model.Title,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    PriorityId = model.PriorityId,
                    DueDate = model.DueDate,
                    CreatedDate = DateTimeOffset.Now,
                    IsCompleted = false,
                };

                _context.TaskItems.Add(taskItem);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Успішно створено завдання: '{model.Title}'.";
                return RedirectToAction(nameof(TaskList));
            }

            await LoadReferenceData(model);
            return View("~/Views/Task/TaskCreator.cshtml", model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Редагування Завдання";
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            var model = new TaskInputModel
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description ?? string.Empty,
                CategoryId = taskItem.CategoryId,
                PriorityId = taskItem.PriorityId,
                DueDate = taskItem.DueDate,
                IsCompleted = taskItem.IsCompleted
            };

            await LoadReferenceData(model);
            return View("~/Views/Task/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskInputModel model)
        {
            ModelState.Remove("AvailableCategories");
            ModelState.Remove("AvailablePriorities");

            if (ModelState.IsValid)
            {
                var taskItem = await _context.TaskItems.FindAsync(model.Id);

                if (taskItem == null)
                {
                    return NotFound();
                }

                taskItem.Title = model.Title;
                taskItem.Description = model.Description;
                taskItem.CategoryId = model.CategoryId;
                taskItem.PriorityId = model.PriorityId;
                taskItem.DueDate = model.DueDate;
                taskItem.IsCompleted = model.IsCompleted;

                _context.TaskItems.Update(taskItem);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Завдання '{model.Title}' успішно оновлено. Статус: {(model.IsCompleted ? "Виконано" : "В роботі")}.";
                return RedirectToAction(nameof(TaskList));
            }

            await LoadReferenceData(model);
            return View("~/Views/Task/Edit.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["Title"] = "Видалення Завдання";
            var task = await GetBaseTaskQuery()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View("~/Views/Task/Delete.cshtml", task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Завдання успішно видалено.";
            }
            return RedirectToAction(nameof(TaskList));
        }

        [HttpGet]
        public async Task<IActionResult> ProgressTracker()
        {
            ViewData["Title"] = "Відстеження Прогресу";

            var tasks = await _context.TaskItems
                                      .Include(t => t.Category)
                                      .Include(t => t.Priority)
                                      .ToListAsync();

            var categories = await _context.Categories.ToDictionaryAsync(c => c.Id, c => c.Name);

            var modelList = tasks.Select(t => new TaskInputModel
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                CategoryId = t.CategoryId,
                DueDate = t.DueDate,

                AvailableCategories = categories
            }).ToList();

            if (TempData.ContainsKey("OutputMessage"))
            {
                ViewData["OutputMessage"] = TempData["OutputMessage"];
            }

            return View("~/Views/Task/ProgressTracker.cshtml", modelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProgress([FromForm] TaskInputModel model)
        {
            if (model.Id == 0)
            {
                TempData["OutputMessage"] = "Помилка: Не вказано ідентифікатор завдання.";
                return RedirectToAction(nameof(ProgressTracker));
            }

            var taskToUpdate = await _context.TaskItems.FindAsync(model.Id);

            if (taskToUpdate == null)
            {
                TempData["OutputMessage"] = $"Помилка: Завдання з ID {model.Id} не знайдено.";
                return RedirectToAction(nameof(ProgressTracker));
            }

            taskToUpdate.IsCompleted = model.IsCompleted;

            try
            {
                await _context.SaveChangesAsync();

                TempData["OutputMessage"] = $"Статус завдання '{taskToUpdate.Title}' успішно оновлено до: {(taskToUpdate.IsCompleted ? "Завершено" : "Активне")}.";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["OutputMessage"] = "Помилка оновлення: Хтось інший змінив завдання. Спробуйте ще раз.";
            }
            catch (Exception ex)
            {
                TempData["OutputMessage"] = $"Не вдалося оновити статус: {ex.Message}";
            }

            return RedirectToAction(nameof(ProgressTracker));
        }


        [HttpGet]
        public IActionResult DeadlineManager()
        {
            ViewData["Title"] = "Складний Пошук Завдань";
            return View("~/Views/Task/DeadlineManager.cshtml", new TaskSearchModel());
        }

        [HttpPost]
        public async Task<IActionResult> DeadlineManager(TaskSearchModel model)
        {
            ViewData["Title"] = "Складний Пошук Завдань";
            model.SearchMessage = "Результати пошуку:";

            var query = GetBaseTaskQuery();

            if (model.DateFrom.HasValue)
            {
                query = query.Where(t => t.DueDate >= model.DateFrom.Value);
            }

            if (model.DateTo.HasValue)
            {
                var dateToExclusive = model.DateTo.Value.AddDays(1);
                query = query.Where(t => t.DueDate < dateToExclusive);
            }

            if (!string.IsNullOrWhiteSpace(model.CategoryNamesCsv))
            {
                var categoryNames = model.CategoryNamesCsv
                    .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => n.Trim())
                    .ToList();

                query = query.Where(t => categoryNames.Contains(t.CategoryName));
            }

            if (!string.IsNullOrWhiteSpace(model.TitleStartsWith))
            {
                query = query.Where(t => t.Title.StartsWith(model.TitleStartsWith));
            }

            if (!string.IsNullOrWhiteSpace(model.TitleEndsWith))
            {
                query = query.Where(t => t.Title.EndsWith(model.TitleEndsWith));
            }

            model.SearchResults = await query.ToListAsync();

            if (model.SearchResults.Count == 0)
            {
                model.SearchMessage = "За вашими критеріями не знайдено жодного завдання.";
            }

            return View("~/Views/Task/DeadlineManager.cshtml", model);
        }
    }
}