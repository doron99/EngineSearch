using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using SearchEngine.Data;
using SearchEngine.Models;
using System.Web;

namespace SearchEngine.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IMemoryCache _cache;
        private readonly ILogRepository _logRepository;

        public CacheRepository(IMemoryCache cache
            , ILogRepository logRepository
            )
        {
            _cache = cache;
            _logRepository = logRepository;
        }
        public async Task<T> GetData<T>(string key) where T : class
        {
            try
            {
                T item = (T)_cache.Get(key);
                return item;
            }
            catch (Exception ex)
            {
                await _logRepository.WriteToLog("LogRepository ex: " + ex.Message);
                return null;
            }
        }


        public async Task<bool> SetData<T>(string key, T value, DateTime expirationTime)
        {
            bool res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _cache.Set(key, value);
                } else
                {
                    res = false;
                }
            } catch (Exception ex)
            {
                await _logRepository.WriteToLog("LogRepository ex: " +ex.Message);
                res = false;
            }
            return res;
        }
    }
}
