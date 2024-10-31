using System.ComponentModel.DataAnnotations;

namespace TournamentProject.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = ".لطفا فیلد مربوطه را وارد کنید")]
        [Display(Name = "نام کاربری")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = ".لطفا فیلد مربوطه را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string? Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}
