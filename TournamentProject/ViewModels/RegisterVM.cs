using System.ComponentModel.DataAnnotations;

namespace TournamentProject.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نیست ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن باید برابر باشد")]
        public string? ConfirmPassword { get; set; }
    }
}
