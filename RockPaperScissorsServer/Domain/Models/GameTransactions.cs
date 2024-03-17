namespace RockPaperScissorsServer.Domain.Models
{
    public class GameTransactions
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public double Amount { get; set; }
    }
}
