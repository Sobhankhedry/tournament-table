namespace TournamentProject.Models
{
    public class SaveBracketRequest
    {
        public string TournamentName { get; set; }
        public List<Bracket> Brackets { get; set; }
    }

}
