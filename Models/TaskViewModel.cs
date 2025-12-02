namespace EduPlanApp.Models
{
    // Модель, що використовується для відображення завдання у списках (центральна таблиця)
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public bool IsCompleted { get; set; }

        // Зв'язки (для відображення назв категорій та пріоритетів)
        public string CategoryName { get; set; } = string.Empty;
        public string PriorityName { get; set; } = string.Empty;
    }
}