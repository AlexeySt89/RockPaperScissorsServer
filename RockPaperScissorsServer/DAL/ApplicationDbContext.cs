using Microsoft.EntityFrameworkCore;
using RockPaperScissorsServer.Domain.Models;

namespace RockPaperScissorsServer.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            /*Database.EnsureDeleted();
            Database.EnsureCreated();*/
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MatchHistory> MatchHistories { get; set; }
        public DbSet<GameTransactions> GameTransactions { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "rostik", Balance = 215125 },
                new User { Id = 2, Name = "bleb", Balance = 12123125 },
                new User { Id = 3, Name = "vova", Balance = 4445 });

            modelBuilder.Entity<GameTransactions>().HasData(
               new GameTransactions { Id = 1, SenderId = 1, RecipientId = 2, Amount = 100 },
               new GameTransactions { Id = 2, SenderId = 2, RecipientId = 3, Amount = 124 },
               new GameTransactions { Id = 3, SenderId = 1, RecipientId = 2, Amount = 100 });

            modelBuilder.Entity<MatchHistory>().HasData(
                new MatchHistory { Id = 1, DateTime = DateTime.UtcNow, Bet = 124, CreaterName = "rostik", CreaterHand = "rock",
                    RivelName = "bleb", RivelHand = "paper", Winner = "rostik", Loser = "bleb" },
                new MatchHistory { Id = 2, DateTime = DateTime.UtcNow, Bet = 412, CreaterName = "bleb", CreaterHand = "sci",
                    RivelName = "rostik", RivelHand = "paper", Winner = "bleb", Loser = "rostik" });
        }*/
    }
}
