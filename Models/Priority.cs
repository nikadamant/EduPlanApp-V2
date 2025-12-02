using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EduPlanApp.Models
{
    // Довідкова таблиця 2 (Directory 2)
    public class Priority
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [Range(1, 3)] // 1: Високий, 2: Середній, 3: Низький
        public int Level { get; set; }

        // Навігаційна властивість
        public virtual ICollection<TaskItem>? Tasks { get; set; }
    }
}