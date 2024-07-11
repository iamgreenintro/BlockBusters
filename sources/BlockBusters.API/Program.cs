using BlockBusters.Service;
using BlockBusters.Service.Domain;
using BlockBusters.Shared;

namespace BlockBusters.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddTransient<VideoDiscoveryService>();
            builder.Services.AddTransient<VideoRepository>();
            builder.Services.AddTransient<IBlockBustersConnection, ApiBlockBusterConnection>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
