using UserMS.DTO;

namespace UserMS.Cache
{
    public interface IUserCacheClient
    {
        bool AddUser(UserDTO userToAdd);
        bool DeleteUser(long id);
        UserDTO? GetUser(long id);
    }
}