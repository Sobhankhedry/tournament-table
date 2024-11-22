using System.ComponentModel.DataAnnotations;

namespace TournamentProject.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage = "وارد کردن این فیلد الزامی است")]
        [MaxLength(15, ErrorMessage = "حداکثر باید 15 کاراکتر باشد")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "وارد کردن این فیلد الزامی است")]
        [Key]
        [MaxLength(30, ErrorMessage = "حداکثر باید 30 کاراکتر باشد")]
        public string? Email { get; set; }
        [MaxLength(200, ErrorMessage = "حد اکثر باید 200 کاراکتر باشد")]
        public string? Description { get; set; }

    }
}
