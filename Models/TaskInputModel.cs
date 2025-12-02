using System.ComponentModel.DataAnnotations;

namespace EduPlanApp.Models
{
    public class TaskInputModel
    {
        // Якщо це редагування, Id буде мати значення
        public int Id { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть назву завдання.")]
        [StringLength(100, ErrorMessage = "Назва не може перевищувати 100 символів.")]
        [Display(Name = "Назва Завдання")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Опис не може перевищувати 500 символів.")]
        [Display(Name = "Опис")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, оберіть категорію.")]
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Будь ласка, оберіть пріоритет.")]
        [Display(Name = "Пріоритет")]
        public int PriorityId { get; set; }

        [Required(ErrorMessage = "Будь ласка, встановіть термін виконання.")]
        [Display(Name = "Кінцевий термін")]
        public DateTimeOffset DueDate { get; set; } = DateTimeOffset.Now.AddDays(7);

        [Display(Name = "Завершено")]
        public bool IsCompleted { get; set; }

        // Додаткові поля для випадаючих списків (заповнюються у контролері)
        public Dictionary<int, string> AvailableCategories { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, string> AvailablePriorities { get; set; } = new Dictionary<int, string>();
    }
}