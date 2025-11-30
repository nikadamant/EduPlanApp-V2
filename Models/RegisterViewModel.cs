using System.ComponentModel.DataAnnotations;

namespace EduPlanApp.Models
{
    public class RegisterViewModel
    {
        // a. Username (50 characters, unique)
        [Required(ErrorMessage = "Ім'я користувача обов'язкове.")]
        [StringLength(50, ErrorMessage = "Ім'я користувача має бути не більше {1} символів.")]
        [Display(Name = "Ім'я користувача (Унікальне)")]
        public string Username { get; set; }

        // b. Full name (500 characters)
        [StringLength(500, ErrorMessage = "Повне ім'я має бути не більше {1} символів.")]
        [Display(Name = "Повне ім'я")]
        public string FullName { get; set; }

        // c. Password and password confirmation
        // Вимоги: 8-16 символів, 1 цифра, 1 знак, 1 велика літера
        [Required(ErrorMessage = "Пароль обов'язковий.")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Пароль має бути від {2} до {1} символів.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[A-Z])(?=.*[^a-zA-Z0-9\s]).{8,16}$",
            ErrorMessage = "Пароль має містити: 1 цифру, 1 велику літеру та 1 спеціальний символ.")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження паролю")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають.")]
        public string ConfirmPassword { get; set; }

        // d. Phone (Ukrainian format) - +380YYXXXXXXX
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+380[0-9]{9}$", ErrorMessage = "Невірний формат телефону. Використовуйте формат +380XXXXXXXXX.")]
        [Display(Name = "Телефон (Український формат)")]
        public string Phone { get; set; }

        // e. RFC 822 Email Address
        [Required(ErrorMessage = "Email обов'язковий.")]
        [EmailAddress(ErrorMessage = "Невірний формат Email.")]
        [Display(Name = "Email (RFC 822)")]
        public string Email { get; set; }
    }
}
