namespace RockPaperScissorsServer.Domain.Models
{
    public class MatchHistory
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public double Bet { get; set; }
        public string CreaterName { get; set; }
        public string CreaterHand { get; set; }
        public string? RivelName { get; set; }
        public string? RivelHand { get; set; }
        public string? Winner { get; set; }
        public string? Loser { get; set; }
    }
}
