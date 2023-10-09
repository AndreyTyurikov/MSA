using Microsoft.EntityFrameworkCore;
using InvoiceMS.Infrastructure.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace InvoiceMS.Infrastructure.Domain.DbCtx
{
    public class InvoiceMsDbContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceEntry> InvoiceEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration AppConfiguration =
                new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

            optionsBuilder.UseSqlServer(AppConfiguration.GetConnectionString("InvoiceMsDb"));
        }
    }
}
