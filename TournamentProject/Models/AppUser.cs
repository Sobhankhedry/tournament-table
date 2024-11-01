using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TournamentProject.Models
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "پر کردن این فیلد الزامی است")]
        [MaxLength(30)]
        public string? Name { get; set; }
    }
}
