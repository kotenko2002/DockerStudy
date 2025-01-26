using API_1.Data;
using Microsoft.EntityFrameworkCore;

namespace API_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _env = builder.Environment;

            builder.Services.AddHttpClient();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            if (_env.IsProduction())
            {
                Console.WriteLine("--> Using PostgreSQL Db");
                builder.Services.AddDbContext<AppDbContext>(opt =>
                    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                Console.WriteLine("--> Using InMem Db");
                builder.Services.AddDbContext<AppDbContext>(opt =>
                     opt.UseInMemoryDatabase("InMem"));
            }

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                    Console.WriteLine("--> Migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> An error occurred while applying migrations: {ex.Message}");
                }
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
