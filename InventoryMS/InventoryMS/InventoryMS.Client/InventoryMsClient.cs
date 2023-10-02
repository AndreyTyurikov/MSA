using Microsoft.Extensions.Configuration;
using Refit;

namespace InventoryMS.Client
{
    public class InventoryMsClient
    {
        public static readonly IInventoryMSClient Client;

        private static readonly string? serviceAddress;

        static InventoryMsClient()
        {
            IConfiguration AppConfiguration =
                new ConfigurationBuilder()
                .AddJsonFile("clientsettings.json").Build();

            serviceAddress = AppConfiguration.GetSection("InventoryServiceAddress").Value;

            Client = RestService.For<IInventoryMSClient>(serviceAddress);
        }
    }
}