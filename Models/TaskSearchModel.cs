using System.ComponentModel.DataAnnotations;

namespace EduPlanApp.Models
{
    // Модель для сторінки пошуку
    public class TaskSearchModel
    {
        [Display(Name = "Пошук за назвою (початок)")]
        public string? TitleStartsWith { get; set; }

        [Display(Name = "Пошук за назвою (кінець)")]
        public string? TitleEndsWith { get; set; }

        [Display(Name = "Дата початку")]
        [DataType(DataType.Date)]
        public DateTimeOffset? DateFrom { get; set; }

        [Display(Name = "Дата кінця")]
        [DataType(DataType.Date)]
        public DateTimeOffset? DateTo { get; set; }

        [Display(Name = "Категорії (через кому)")]
        public string? CategoryNamesCsv { get; set; }

        // Результати пошуку
        public List<TaskViewModel> SearchResults { get; set; } = new List<TaskViewModel>();

        public string SearchMessage { get; set; } = "Введіть параметри для пошуку";
    }
}