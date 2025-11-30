namespace EduPlanApp.Models
{
    public class TaskInputModel
    {
        public string TaskName { get; set; }
        public string TaskType { get; set; } // Лаб., Проєкт, Іспит
        public DateTime Deadline { get; set; } = DateTime.Now.AddDays(7);
        public string Description { get; set; }
        public string Status { get; set; } // В роботі, Готово, Відкладено
        public string OutputMessage { get; set; } // Поле для виведення результату
    }
}
