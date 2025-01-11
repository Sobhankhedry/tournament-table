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
        public DbSet<Team> Teams { get; set; }
        public DbSet<Medals> Medals { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Confirming> Comfirm { get; set; }
        public DbSet<MatchEntity> Matches { get; set; }
        public DbSet<Referees> Referee { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
    }

}
