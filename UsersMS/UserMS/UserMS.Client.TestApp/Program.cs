// See https://aka.ms/new-console-template for more information

using UserMS.Cache;
using UserMS.DTO;

var userDTO = new UserDTO()
{
      Id = 1,
      Name = "First Test User",
      Email = "first@company.com",
      BirthDate = DateTime.Now.AddYears(-30),
      PhoneNumber = "+148834432123",
      registeredAt = DateTime.Now
};


var cacheClient = new UserCacheClient();

cacheClient.AddUser(userDTO);

//cacheClient.AddUser(userDTO);

//cacheClient.DeleteUser(1);
