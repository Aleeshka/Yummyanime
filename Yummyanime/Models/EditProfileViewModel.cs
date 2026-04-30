using System.ComponentModel.DataAnnotations;

namespace Yummyanime.Models
{
    public class EditProfileViewModel
    {
        [Required]
        [Display(Name = "Отображаемое имя")]
        public string? DisplayName { get; set; }

        public string? CurrentAvatarFileName { get; set; }
    }
}