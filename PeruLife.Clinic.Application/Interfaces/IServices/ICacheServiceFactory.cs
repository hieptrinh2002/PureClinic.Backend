using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface ICacheServiceFactory
    {
        ICacheService GetCacheService(CacheType cacheType);
    }
}
