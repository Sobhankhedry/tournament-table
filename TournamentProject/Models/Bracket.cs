namespace TournamentProject.Models
{
    public class Bracket
    {
        public int BracketNo { get; set; }
        public int RoundNo { get; set; }
        public string[] Teamnames { get; set; }
        public int[] Scores { get; set; }
        public int? NextGame { get; set; }
        public int[] LastGames { get; set; }
        public string TournamentName { get; internal set; }
    }
}
