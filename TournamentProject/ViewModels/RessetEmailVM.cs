using System.ComponentModel.DataAnnotations;

namespace TournamentProject.ViewModels
{
    public class RessetEmailVM
    {
        [Required(ErrorMessage = ".لطفا فیلد مربوطه را وارد کنید")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }


    }
}
