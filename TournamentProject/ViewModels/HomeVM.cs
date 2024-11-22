using TournamentProject.Models;

namespace TournamentProject.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Medals>? Medals1 { get; set; }
        public IEnumerable<Referee>? Referees1 { get; set; }
        public IEnumerable<Coach>? Coaches1 { get; set; }
        public IEnumerable<Team>? Championships1 { get; set; }
    }
}
