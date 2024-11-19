using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TournamentProject.Models;

namespace TournamentProject.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Referee> Referees { get; set; }
    }

}
