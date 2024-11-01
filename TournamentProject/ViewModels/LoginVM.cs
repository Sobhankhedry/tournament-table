using System.ComponentModel.DataAnnotations;

namespace TournamentProject.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = ".لطفا فیلد مربوطه را وارد کنید")]
        [Display(Name = "ایمیل")]
        public string? Email { get; set; }

        [Required(ErrorMessage = ".لطفا فیلد مربوطه را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string? Password { get; set; }

        [Display(Name = "به خاطر سپردن نام کاربری")]
        public bool RememberMe { get; set; }
    }
}
