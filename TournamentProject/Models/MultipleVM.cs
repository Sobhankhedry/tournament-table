using TournamentProject.ViewModels;

namespace TournamentProject.Models
{
    public class MultipleVM
    {
        public ContactUs? ContactUs { get; set; }
        public HomeVM? HomeVM { get; set; }
        public List<Referee>? Referees { get; set; }
        public List<Coach>? Coaches { get; set; }
        public List<Team>? Teams { get; set; }
        public List<Medals>? Medal { get; set; }
    }
}
