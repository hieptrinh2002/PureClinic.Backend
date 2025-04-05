using Microsoft.Extensions.Options;
using PureLifeClinic.Application.Interfaces.IQueue;
using PureLifeClinic.Core.Common;
using StackExchange.Redis;

namespace PureLifeClinic.Infrastructure.ExternalServices
{
    public class RedisQueueService : IRedisQueueService
    {
        private readonly IDatabase _redis;
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly DatabaseSettings _dbSettings;

        public RedisQueueService(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
            _redisConnection = ConnectionMultiplexer.Connect(_dbSettings.RedisConnection);
            _redis = _redisConnection.GetDatabase();
        }

        public async Task AddToConsultationQueueAsync(string queueNumber)
        {
            var key = $"consultation:queue:{DateTime.Today:yyyyMMdd}";
            await _redis.SortedSetAddAsync(key, queueNumber, DateTime.UtcNow.Ticks);
        }

        public async Task<string?> GetNextConsultationAsync()
        {
            var key = $"consultation:queue:{DateTime.Today:yyyyMMdd}";
            var result = await _redis.SortedSetRangeByRankAsync(key, 0, 0);
            if (result.Length == 0) 
                return null;

            await _redis.SortedSetRemoveAsync(key, result[0]);
            return result[0].ToString();
        }

        public async Task<List<string>> GetConsultationQueueSnapshot(int pageNumber, int pageSize)
        {
            var key = $"consultation:queue:{DateTime.Today:yyyyMMdd}";

            int start = (pageNumber - 1) * pageSize;
            int stop = start + pageSize - 1;

            var results = await _redis.SortedSetRangeByRankAsync(key, start, stop);

            if (results.Length == 0)
                return new List<string>();

            return results.Select(x => x.ToString()).ToList();
        }

        public async Task AddToExaminationQueueAsync(string queueNumber, int doctorId, int priority)
        {
            var key = $"examination:queue:{doctorId}";

            var currentTime = DateTime.UtcNow;

            // 1(int) priority = 10 phút (600 giây)
            var priorityAdjustment = priority * 10 * 60; 
            var score = currentTime.AddSeconds(priorityAdjustment).Ticks;

            var result = await _redis.SortedSetAddAsync(key, queueNumber, score);

            if (!result)
            {
                throw new InvalidOperationException("Failed to add to the examination queue.");
            }
        }

        public async Task<string?> GetNextExaminationAsync(int doctorId)
        {
            var key = $"examination:queue:{doctorId}";
            var result = await _redis.SortedSetRangeByRankAsync(key, 0, 0);
            if (result.Length == 0) 
                return null;

            await _redis.SortedSetRemoveAsync(key, result[0]);
            return result[0].ToString();
        }

        public async Task<List<string>> GetExaminationQueueSnapshot(int doctorId)
        {
            var key = $"examination:queue:{doctorId}";
            var results = await _redis.SortedSetRangeByRankAsync(key, 0, -1);
            if (results.Length == 0)
                return new List<string>();
            return results.Select(x => x.ToString()).ToList();
        }
    }
}
