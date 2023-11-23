using DiscountMS.Host.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DiscountMS.Host.Domain.DbCtx
{
    public class DiscountServiceDbContext : DbContext
    {
        public DbSet<DiscountType> DiscountTypes { get; set; }
        public DbSet<DiscountAmountType> DiscountAmountTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration AppConfiguration =
                new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

            optionsBuilder.UseNpgsql(AppConfiguration.GetConnectionString("DiscountMsDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DiscountType[] discountTypes = { 
                new DiscountType { DiscountTypeId = 1, DiscountTypeName = "Personal" },
                new DiscountType { DiscountTypeId = 2, DiscountTypeName = "InventoryItem" },
                new DiscountType { DiscountTypeId = 3, DiscountTypeName = "FromInvoiceTotal" },
                new DiscountType { DiscountTypeId = 4, DiscountTypeName = "Sale" },
                new DiscountType { DiscountTypeId = 5, DiscountTypeName = "InventoryItemUponInvoiceAmount" }
            };

            modelBuilder.Entity<DiscountType>().HasData(discountTypes);

            DiscountAmountType[] discountAmountTypes = {
                new DiscountAmountType { DiscountAmountTypeId = 1, DiscountAmountTypeName = "FixedAmount" },
                new DiscountAmountType { DiscountAmountTypeId = 2, DiscountAmountTypeName = "Percentage" }
            };

            modelBuilder.Entity<DiscountAmountType>().HasData(discountAmountTypes);
        }
    }
}
