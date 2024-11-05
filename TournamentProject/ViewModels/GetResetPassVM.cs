using System.ComponentModel.DataAnnotations;

namespace TournamentProject.ViewModels
{
    public class GetResetPassVM
    {
        public string UserId { get; set; }
        public string? Token { get; set; }
        [Required(ErrorMessage = ".لطفا فیلد مربوطه را وارد کنید")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = ".لطفا فیلد مربوطه را وارد کنید")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن باید برابر باشد")]
        public string? ConfirmPassword { get; set; }
    }
}
