using Crud;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using RockPaperScissorsServer.DAL;
using RockPaperScissorsServer.Domain.Models;

namespace RockPaperScissorsServer.Services
{
    public class UserApiService : UserService.UserServiceBase
    {
        private readonly ApplicationDbContext _context;
        public UserApiService(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var user = new User { Name = request.Name };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var reply = new UserReply() { Id = user.Id, Name = user.Name, Balance = user.Balance };
            return await Task.FromResult(reply);
        }

        public override async Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var user = await _context.Users.Where(r => r.Name == request.OldName).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Пользователь не найден."));
            }
            user.Name = request.NewName;
            await _context.SaveChangesAsync();
            var reply = new UserReply() { Id = user.Id, Name = user.Name, Balance = user.Balance };
            return await Task.FromResult(reply);
        }

        public override async Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var user = await _context.Users.Where(r => r.Name == request.Name).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Пользователь не найден."));
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            var reply = new UserReply() { Id = user.Id, Name = user.Name, Balance = user.Balance };
            return await Task.FromResult(reply);
        }
    }
}
