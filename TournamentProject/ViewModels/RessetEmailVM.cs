using System.ComponentModel.DataAnnotations;

namespace TournamentProject.ViewModels
{
    public class RessetEmailVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }


    }
}
