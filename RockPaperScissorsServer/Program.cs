
using Microsoft.EntityFrameworkCore;
using RockPaperScissorsServer.DAL;
using RockPaperScissorsServer.Services;

namespace RockPaperScissorsServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();
            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connection));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<UserApiService>();
            app.MapGrpcService<MacthApiService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}