namespace RockPaperScissorsServer.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public ICollection<GameTransactions> GameTransactions { get; set; }
        public ICollection<MatchHistory> MatchHistories { get; set; }
    }
}
