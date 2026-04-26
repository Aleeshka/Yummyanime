using System.ComponentModel.DataAnnotations;

namespace Yummyanime.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        public string? ConfirmPassword { get; set; }
    }
}