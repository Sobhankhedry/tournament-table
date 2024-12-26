namespace TournamentProject.ViewModels
{
    public class BracketMatchViewModel
    {
        public int BracketNo { get; set; }
        public int RoundNo { get; set; }
        public List<string> TeamNames { get; set; }
        public List<int> Scores { get; set; }
        public int? NextGame { get; set; }
        public List<int?> LastGames { get; set; }
    }
}
