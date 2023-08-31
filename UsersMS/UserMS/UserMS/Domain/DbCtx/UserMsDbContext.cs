using Microsoft.EntityFrameworkCore;
using UserMS.Domain.Models;

namespace UserMS.Domain.DbCtx
{
    public class UserMsDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration AppConfiguration =
                new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

            optionsBuilder.UseSqlServer(AppConfiguration.GetConnectionString("UserMsDb"));
        }

        public DbSet<User> Users { get; set; } 
    }
}
