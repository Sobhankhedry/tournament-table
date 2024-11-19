using System.ComponentModel.DataAnnotations;

namespace TournamentProject.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage = "وارد کردن این فیلد الزامی است")]
        [MaxLength(15)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "وارد کردن این فیلد الزامی است")]
        [Key]
        [MaxLength(30)]
        public string? Email { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }

    }
}
