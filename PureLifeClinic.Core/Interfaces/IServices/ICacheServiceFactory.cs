using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Services;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface ICacheServiceFactory
    {
        ICacheService GetCacheService(CacheType cacheType);
    }
}
