using StackExchange.Redis;
using System.Text.Json;
using UserMS.DTO;

namespace UserMS.CacheClient
{
    public class UserMsCacheClient : IUserMsCacheClient
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _redisDb;

        public UserMsCacheClient()
        {
            _redisConnection = ConnectionMultiplexer.Connect("172.18.0.5");
            _redisDb = _redisConnection.GetDatabase(0);
        }

        ~UserMsCacheClient()
        {
            _redisConnection.Close();
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

        private RedisKey BuildUserKey(long id)
        {
            return new RedisKey($"User: {id}");
        }
    }
}