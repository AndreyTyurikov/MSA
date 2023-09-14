using UserMS.Domain.Models;

namespace UserMS.Domain.DataLayer
{
    public interface IUserDataLayer
    {
        Task<User> Add(User newUser);
        Task<User> GetByID(long id);
    }
}
