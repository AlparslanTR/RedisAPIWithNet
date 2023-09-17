using StackExchange.Redis;

namespace Net_RedisExchangeAPI.Services
{
    public class RedisService
    {
        private readonly IConfiguration _configuration;

        private readonly string _Host;
        private readonly string _Port;
        public IDatabase _database;
        private ConnectionMultiplexer _redis;

        public RedisService(IConfiguration configuration)
        {
            _Host = configuration["Redis:Host"];
            _Port = configuration["Redis:Port"];
        }

        public void Connect()
        {
            var configString = $"{_Host}:{_Port}";
            _redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDatabase(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
