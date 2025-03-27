using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Infrastructure.Caching
{
    public class CacheServiceFactory : ICacheServiceFactory
    {
        private readonly ICacheService _memoryCacheService;
        private readonly ICacheService _redisCacheService;

        public CacheServiceFactory(MemoryCacheService memoryCacheService, RedisCacheService redisCacheService)
        {
            _memoryCacheService = memoryCacheService;
            _redisCacheService = redisCacheService;
        }

        public ICacheService GetCacheService(CacheType cacheType)
        {
            return cacheType switch
            {
                CacheType.Memory => _memoryCacheService,
                CacheType.Redis => _redisCacheService,
                _ => throw new ArgumentException("Invalid cache type specified")
            };
        }
    }
}
