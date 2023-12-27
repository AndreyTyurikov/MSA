using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System.Text.Json;
using UserMS.DTO;

namespace UserMS.Cache
{
    public class UserCacheClient : IUserCacheClient
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _redisDb;

        public UserCacheClient()
        {
            _redisConnection = ConnectionMultiplexer.Connect("localhost");
            _redisDb = _redisConnection.GetDatabase(0);
        }

        ~UserCacheClient()
        {
            _redisConnection.Close();
        }

        public bool AddUser(UserDTO userToAdd)
        {
            bool isUserAdded = false;

            string userJson = JsonSerializer.Serialize(userToAdd);

            try
            {
                _redisDb.StringSet(BuildUserKey(userToAdd.Id), new RedisValue(userJson));

                isUserAdded = true;
            }
            catch (Exception)
            {
                throw;
            }

            return isUserAdded;
        }

        public UserDTO? GetUser(long id)
        {
            UserDTO? userById = null;

            try
            {
                RedisValue objectById = _redisDb.StringGet(BuildUserKey(id));

                if (objectById.HasValue) userById = JsonSerializer.Deserialize<UserDTO>(objectById);
            }
            catch (Exception)
            {
                throw;
            }

            return userById;
        }

        public bool DeleteUser(long id)
        {
            bool isUserDeleted = false;

            try
            {
                _redisDb.KeyDelete(BuildUserKey(id));

                isUserDeleted = true;
            }
            catch (Exception)
            {
                throw;
            }

            return isUserDeleted;
        }

        private RedisKey BuildUserKey(long id)
        {
            return new RedisKey($"User: {id}");
        }
    }
}
