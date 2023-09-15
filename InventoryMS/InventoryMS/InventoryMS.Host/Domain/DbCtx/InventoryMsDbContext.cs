using InventoryMS.Host.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryMS.Host.Domain.DbCtx
{
    public class InventoryMsDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration AppConfiguration =
                new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

            optionsBuilder.UseSqlServer(AppConfiguration.GetConnectionString("InventoryMsDb"));
        }

        public DbSet<InventoryItem> InventoryItems { get; set; }
    }
}
