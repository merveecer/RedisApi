using System.Text.Json;
using RedisApi.Models;
using StackExchange.Redis;

namespace RedisApi.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;
        public PlatformRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentOutOfRangeException(nameof(platform));
            }
            var db = _redis.GetDatabase();

            var serialPlatform = JsonSerializer.Serialize(platform);
            db.HashSet($"hashPlatForm", new HashEntry[]{
                new HashEntry(platform.Id,serialPlatform)
            });
            // throw new NotImplementedException();
        }

        public IEnumerable<Platform?>? GetAllPlatforms()
        {
            var db = _redis.GetDatabase();
            var allSet = db.HashGetAll("hashPlatForm");
            if (allSet.Length > 0)
            {
                var setObj = Array.ConvertAll(allSet, (a) => JsonSerializer.Deserialize<Platform>(a.Value)).ToList();
                return setObj;
            }
            return null;
        }

        public Platform? GetPlatformById(string Id)
        {

            var db = _redis.GetDatabase();
            var platform = db.HashGet("hashPlatForm", Id);
            if (!string.IsNullOrEmpty(platform))
            {
                return JsonSerializer.Deserialize<Platform>(platform);
            }
            return null;
        }
    }
}