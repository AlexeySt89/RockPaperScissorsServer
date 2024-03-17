using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using RockPaperScissorsServer.DAL;
using RockPaperScissorsServer.Domain.Models;
using RockPaperScissorsServer.Protos;

namespace RockPaperScissorsServer.Services
{
    public class MacthApiService : MatchService.MatchServiceBase
    {
        private readonly ApplicationDbContext _context;

        public MacthApiService(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task<UserData> CheckBalance(GetUserReq request, ServerCallContext context)
        {
            var user = await _context.Users.Where(item => item.Name.Contains(request.Name)).ToListAsync();

            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            UserData userData = null;
            foreach (var item in user)
            {
                userData = new UserData() { Id = item.Id, Name = item.Name, Balance = item.Balance };
            }
            return await Task.FromResult(userData);
        }

        public override async Task<UserData> MoneyTrans(CreateTrans request, ServerCallContext context)
        {
            var sender = await _context.Users.Where(r => r.Name == request.SenderName).FirstOrDefaultAsync();
            if (sender == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Пользователь не найден."));

            }
            if (sender.Balance < request.AmountMoney)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Недостаточно средств."));
            }
            sender.Balance = sender.Balance - request.AmountMoney;

            var recipient = await _context.Users.Where(r => r.Name == request.RecipientName).FirstOrDefaultAsync();
            if (recipient == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Пользователь не найден."));

            }
            recipient.Balance = recipient.Balance + request.AmountMoney;

            var trans = new GameTransactions { SenderId = sender.Id, RecipientId = recipient.Id, Amount = request.AmountMoney };
            _context.GameTransactions.AddAsync(trans);
            await _context.SaveChangesAsync();
            var userData = new UserData { Id = sender.Id, Name = sender.Name, Balance = sender.Balance };
            return await Task.FromResult(userData);
        }

        public override async Task<MatchData> CreateMatch(CreateMatchRequest request, ServerCallContext context)
        {
            var sender = await _context.Users.Where(r => r.Name == request.CreaterName).FirstOrDefaultAsync();
            if (sender == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Пользователь не найден."));
            }
            if (sender.Balance < request.Bet)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Недостаточно средств."));
            }
            sender.Balance = sender.Balance - request.Bet;

            var dateTime = DateTime.UtcNow;
            var match = new MatchHistory { DateTime = dateTime, Bet = request.Bet, CreaterName = request.CreaterName, CreaterHand = request.CreaterHand };

            await _context.MatchHistories.AddAsync(match);
            await _context.SaveChangesAsync();
            var reply = new MatchData() { Id = match.Id, DateTime = Timestamp.FromDateTime(dateTime), Bet = match.Bet, CreaterName = match.CreaterName, CreaterHand = match.CreaterHand };
            return await Task.FromResult(reply);
        }

        public override async Task<ListData> ListMatches(Empty request, ServerCallContext context)
        {
            var listReply = new ListData();

            var userList = _context.MatchHistories.Select(item => new MatchData
            {
                Id = item.Id,
                DateTime = Timestamp.FromDateTime(item.DateTime),
                Bet = item.Bet,
                CreaterName = item.CreaterName,
                CreaterHand = item.CreaterHand,
                RivelName = item.RivelName,
                RivelHand = item.RivelHand,
                Winner = item.Winner,
                Loser = item.Loser
            }).ToList();
            listReply.Matches.AddRange(userList);
            return await Task.FromResult(listReply);
        }

        public override async Task<MatchData> MatchConnect(RivelData request, ServerCallContext context)
        {
            var match = await _context.MatchHistories.FindAsync(request.MatchId);
            if (match == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Матч не найден"));
            }
            match.RivelName = request.RivelName;
            match.RivelHand = request.RivelHand;

            var player1 = await _context.Users.Where(x => x.Name == match.CreaterName).FirstOrDefaultAsync();
            var player2 = await _context.Users.Where(x => x.Name == match.RivelName).FirstOrDefaultAsync();

            switch (request.RivelHand)
            {
                case "rock":
                    if (match.CreaterHand == "rock")
                    {
                        Console.WriteLine("Draw");
                        Console.WriteLine();
                    }
                    if (match.CreaterHand == "paper")
                    {
                        player2.Balance -= match.Bet;
                        player1.Balance += 2 * match.Bet;
                        match.Winner = player1.Name;
                        match.Loser = player2.Name;
                        Console.WriteLine("You've lost");
                        Console.WriteLine();
                    }
                    if (match.CreaterHand == "sci")
                    {
                        player2.Balance += 2 * match.Bet;
                        match.Winner = player2.Name;
                        match.Loser = player1.Name;
                        Console.WriteLine("You've won");
                        Console.WriteLine();
                    }
                    break;

                case "paper":
                    if (match.CreaterHand == "paper")
                    {
                        Console.WriteLine("Draw");
                        Console.WriteLine();
                    }
                    if (match.CreaterHand == "sci")
                    {
                        player2.Balance -= match.Bet;
                        player1.Balance += 2 * match.Bet;
                        match.Winner = player1.Name;
                        match.Loser = player2.Name;
                        Console.WriteLine("You've lost");
                        Console.WriteLine();
                    }
                    if (match.CreaterHand == "rock")
                    {
                        player2.Balance += 2 * match.Bet;
                        match.Winner = player2.Name;
                        match.Loser = player1.Name;
                        Console.WriteLine("You've won");
                        Console.WriteLine();
                    }
                    break;

                case "sci":
                    if (match.CreaterHand == "sci")
                    {
                        Console.WriteLine("Draw");
                        Console.WriteLine();
                    }
                    if (match.CreaterHand == "rock")
                    {
                        player2.Balance -= match.Bet;
                        player1.Balance += 2 * match.Bet;
                        match.Winner = player1.Name;
                        match.Loser = player2.Name;
                        Console.WriteLine("You've lost");
                        Console.WriteLine();
                    }
                    if (match.CreaterHand == "paper")
                    {
                        player2.Balance += 2 * match.Bet;
                        match.Winner = player2.Name;
                        match.Loser = player1.Name;
                        Console.WriteLine("You've won");
                        Console.WriteLine();
                    }
                    break;
            }
            await _context.SaveChangesAsync();
            var reply = new MatchData()
            {
                Id = match.Id,
                DateTime = Timestamp.FromDateTime(match.DateTime),
                Bet = match.Bet,
                CreaterName = match.CreaterName,
                CreaterHand = match.CreaterHand,
                RivelName = request.RivelName,
                RivelHand = request.RivelHand,
                Winner = match.Winner,
                Loser = match.Loser
            };
            return await Task.FromResult(reply);

        }
    }
}
