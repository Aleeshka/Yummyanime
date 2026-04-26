using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Yummyanime.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        [MaxLength(100)]
        public string? DisplayName { get; set; }

        [MaxLength(300)]
        public string? AvatarFileName { get; set; }
    }
}