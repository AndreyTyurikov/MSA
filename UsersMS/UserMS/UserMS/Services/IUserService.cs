using UserMS.DTO;

namespace UserMS.Services
{
    public interface IUserService
    {
        Task<UserDTO> CreateUserFromDto(AddUserDTO userToAdd);
        Task<UserDTO> GetUserById(long id);
    }
}
