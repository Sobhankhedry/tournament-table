namespace TournamentProject.Models
{
    public class MatchEntity
    {
        public int Id { get; set; }
        public string TournamentName { get; set; }
        public int BracketNo { get; set; }
        public int RoundNo { get; set; }
        public string TeamAName { get; set; }
        public string TeamBName { get; set; }
        public int TeamAScore { get; set; }
        public int TeamBScore { get; set; }
        public int? NextGameId { get; set; }

    }
}
