using DiscountMS.Host.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DiscountMS.Host.Domain.DbCtx
{
    public class DiscountServiceDbContext : DbContext
    {
        //Dictionary Tables
        public DbSet<DiscountType> DiscountTypes { get; set; }
        public DbSet<DiscountAmountType> DiscountAmountTypes { get; set; }
        public DbSet<DiscountTerminationType> DiscountTerminationTypes { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }

        //DiscountType specific Tables
        public DbSet<PersonalDiscount> PersonalDiscounts { get; set; }
        public DbSet<InventoryItemDiscount> InventoryItemDiscounts { get; set; }
        public DbSet<InvoiceTotalDiscount> InvoiceTotalDiscounts { get; set; }
        public DbSet<SaleDiscount> SaleDiscounts { get; set; }
        public DbSet<InventoryItemOnInvoiceTotalDiscount> InventoryItemOnInvoiceTotalDiscounts { get; set; }

        //Main discount table
        public DbSet<Discount> Discounts { get; set; }

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

            DiscountTerminationType[] discountTerminationTypes =
            {
                new DiscountTerminationType { DiscountTerminationTypeId = 1, DiscountTerminationTypeName = "SpecificDate" },
                new DiscountTerminationType { DiscountTerminationTypeId = 2, DiscountTerminationTypeName = "OutOfStock" },
                new DiscountTerminationType { DiscountTerminationTypeId = 3, DiscountTerminationTypeName = "Never" }
            };

            modelBuilder.Entity<DiscountTerminationType>().HasData(discountTerminationTypes);

            SaleType[] saleTypes =
            {
                new SaleType { SaleTypeId = 1, SaleTypeName = "Opening" },
                new SaleType { SaleTypeId = 2, SaleTypeName = "Seasoned" },
                new SaleType { SaleTypeId = 3, SaleTypeName = "Holiday" },
                new SaleType { SaleTypeId = 4, SaleTypeName = "Final" }
            };

            modelBuilder.Entity<SaleType>().HasData(saleTypes);
        }
    }
}
