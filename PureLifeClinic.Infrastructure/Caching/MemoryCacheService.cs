﻿using Microsoft.Extensions.Caching.Memory;
using PureLifeClinic.Application.Interfaces.IServices;

namespace PureLifeClinic.Infrastructure.Caching
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetAsync<T>(string cacheKey)
        {
            return await Task.FromResult(_memoryCache.TryGetValue(cacheKey, out T value) ? value : default);
        }

        public async Task SetAsync<T>(string cacheKey, T value, TimeSpan expirationTime)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            };
            _memoryCache.Set(cacheKey, value, cacheEntryOptions);

            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
            await Task.CompletedTask;
        }
    }
}
