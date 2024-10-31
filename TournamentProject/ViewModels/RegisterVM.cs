using System.ComponentModel.DataAnnotations;

namespace TournamentProject.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "وارد کردن این فیلد الزامی است")]
        public string? Name { get; set; }
        public string? Email { get; set; }
        [Compare("Password", ErrorMessage = "رمز عبور اشتباه است ")]
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

    }
}
