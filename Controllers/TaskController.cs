using EduPlanApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduPlanApp.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        [HttpGet]
        public IActionResult TaskCreator()
        {
            ViewData["Description"] = "Створення нового навчального завдання (лабораторна, проєкт, іспит) та встановлення терміну.";
            return View(new TaskInputModel());
        }

        [HttpPost]
        public IActionResult TaskCreator(TaskInputModel model)
        {
            model.OutputMessage = $"Успішно створено завдання: '{model.TaskName}' (Тип: {model.TaskType}) з терміном до {model.Deadline.ToShortDateString()}.";
            ViewData["Description"] = "Створення нового навчального завдання (лабораторна, проєкт, іспит) та встановлення терміну.";
            return View(model);
        }

        [HttpGet]
        public IActionResult ProgressTracker()
        {
            ViewData["Description"] = "Відстеження та оновлення статусу виконання існуючих навчальних завдань.";
            var placeholderTask = new TaskInputModel { TaskName = "Лабораторна 3 (EduPlan)", Status = "В роботі" };
            return View(placeholderTask);
        }

        [HttpPost]
        public IActionResult ProgressTracker(TaskInputModel model)
        {
            model.OutputMessage = $"Статус завдання '{model.TaskName}' успішно оновлено на: '{model.Status}'.";
            ViewData["Description"] = "Відстеження та оновлення статусу виконання існуючих навчальних завдань.";
            return View(model);
        }

        [HttpGet]
        public IActionResult DeadlineManager()
        {
            ViewData["Description"] = "Пошук та фільтрація завдань за найближчими термінами (наприклад, Наступні 7 днів).";
            return View(new TaskInputModel { OutputMessage = "Введіть діапазон дат для пошуку." });
        }

        [HttpPost]
        public IActionResult DeadlineManager(TaskInputModel model)
        {
            model.OutputMessage = $"Знайдено 2 завдання з терміном виконання між {DateTime.Now.ToShortDateString()} та {model.Deadline.ToShortDateString()}.";
            ViewData["Description"] = "Пошук та фільтрація завдань за найближчими термінами (наприклад, Наступні 7 днів).";
            return View(model);
        }
    }
}
