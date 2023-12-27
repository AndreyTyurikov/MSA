using UserMS.DTO;

namespace UserMS.CacheClient
{
    public interface IUserMsCacheClient
    {
        UserDTO? GetUser(long id);
    }
}