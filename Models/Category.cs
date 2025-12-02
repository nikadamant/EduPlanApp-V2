using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EduPlanApp.Models
{
    // Довідкова таблиця 1 (Directory 1)
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        // Навігаційна властивість
        public virtual ICollection<TaskItem>? Tasks { get; set; }
    }
}