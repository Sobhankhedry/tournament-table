namespace TournamentProject.ViewModels
{
    public class TournamentVM
    {
        public string TournamentName { get; set; }
        public string AgeGroup { get; set; }
        public string Gender { get; set; }
        public string WeightClass { get; set; }
        public List<BracketMatchViewModel> Brackets { get; set; }
    }
}
