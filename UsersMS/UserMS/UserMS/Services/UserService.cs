using Mapster;
using UserMS.Domain.DataLayer;
using UserMS.Domain.Models;
using UserMS.DTO;

namespace UserMS.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserDataLayer _userDataLayer;   

        public UserService(IPasswordService passwordService, IUserDataLayer userDataLayer)
        {
            _passwordService = passwordService;
            _userDataLayer = userDataLayer;
        }

        public async Task<UserDTO> CreateUserFromDto(AddUserDTO userToAdd)
        {
            User newUser = userToAdd.Adapt<User>();

            newUser.PasswordHash = _passwordService.CreatePasswordHash(userToAdd.Password);

            User addedUser = await _userDataLayer.Add(newUser);

            UserDTO userDtoToReturn = addedUser.Adapt<UserDTO>();

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
