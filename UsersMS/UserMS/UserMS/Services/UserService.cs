using Mapster;
using UserMS.Cache;
using UserMS.Domain.DataLayer;
using UserMS.Domain.Models;
using UserMS.DTO;

namespace UserMS.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserDataLayer _userDataLayer;
        private readonly IUserCacheClient _userCacheClient;

        public UserService(
            IPasswordService passwordService, 
            IUserDataLayer userDataLayer,
            IUserCacheClient userCacheClient
            )
        {
            _passwordService = passwordService;
            _userDataLayer = userDataLayer;
            _userCacheClient = userCacheClient;
        }

        public async Task<UserDTO> CreateUserFromDto(AddUserDTO userToAdd)
        {
            User newUser = userToAdd.Adapt<User>();

            newUser.PasswordHash = _passwordService.CreatePasswordHash(userToAdd.Password);

            User addedUser = await _userDataLayer.Add(newUser);

            UserDTO userDtoToReturn = addedUser.Adapt<UserDTO>();

            _userCacheClient.AddUser(userDtoToReturn);

            return userDtoToReturn;
        }

        public async Task<UserDTO> GetUserById(long id)
        {
            User userByID = await _userDataLayer.GetByID(id);

            UserDTO userDTO = userByID.Adapt<UserDTO>();

            return userDTO;
        }
    }
}
