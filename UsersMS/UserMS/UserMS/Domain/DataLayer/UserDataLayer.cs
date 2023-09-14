using UserMS.Domain.Models;
using UserMS.Domain.DbCtx;
using Microsoft.EntityFrameworkCore;

namespace UserMS.Domain.DataLayer
{
    public class UserDataLayer : IUserDataLayer
    {
        public async Task<User> Add(User newUser)
        {
            using (UserMsDbContext dbContext = new UserMsDbContext())
            {
                dbContext.Users.Add(newUser);

                await dbContext.SaveChangesAsync();

                return newUser;
            }
        }

        public async Task<User> GetByID(long id)
        {
            using (UserMsDbContext dbContext = new UserMsDbContext())
            {
                User? userById = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                return userById != null ? userById : new User();
            }
        }
    }
}
