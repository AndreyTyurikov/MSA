using Microsoft.Extensions.Configuration;
using System.Net.Http;
using UserMS.DTO;

namespace UserMS.Client
{
    public class UserMsClient : IUserMsClient
    {
        private readonly string? serviceAddress;
        private readonly string? apiBaseAddress;
        private readonly bool configurationOK;
        private readonly HttpClient httpClient;

        public UserMsClient()
        {
            IConfiguration AppConfiguration =
                new ConfigurationBuilder()
                .AddJsonFile("clientsettings.json").Build();

            serviceAddress = AppConfiguration.GetSection("UserServiceAddress").Value;
            apiBaseAddress = AppConfiguration.GetSection("UserServiceApiBase").Value;

            configurationOK = !string.IsNullOrEmpty(serviceAddress) && !string.IsNullOrEmpty(apiBaseAddress);

            httpClient = new HttpClient();
        }

        public Task<UserDTO> CreateUser(AddUserDTO addUserDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> GetUserByID(long id)
        {
            if (configurationOK)
            {
                string requestAddress = $"{serviceAddress}/{apiBaseAddress}/{id}";

                var userByIdJson = await httpClient.GetAsync(requestAddress);

                //TODO: Create UserDTO object from JSON
            }

            return null;
        }
    }
}