using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TournamentProject.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
