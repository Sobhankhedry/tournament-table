using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TournamentProject.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
}

