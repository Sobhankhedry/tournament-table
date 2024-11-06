using System.ComponentModel.DataAnnotations;

namespace TournamentProject.Models
{
    public class ContactUs
    {
        [Required]
        [MaxLength(15)]
        public string? Name { get; set; }
        [Required]
        [Key]
        [MaxLength(30)]
        public string? Email { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }

    }
}
