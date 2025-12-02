using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduPlanApp.Models
{
    // Центральна таблиця для завдань
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле Назва є обов'язковим.")]
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Створено")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Display(Name = "Кінцевий термін")]
        public DateTimeOffset DueDate { get; set; }

        [Display(Name = "Завершено")]
        public bool IsCompleted { get; set; }

        // Зовнішні ключі
        public int CategoryId { get; set; }
        public int PriorityId { get; set; }

        // Навігаційні властивості (JOIN операції)
        public virtual Category? Category { get; set; }
        public virtual Priority? Priority { get; set; }
    }
}