using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace DM.Infrastructure.Cache
{
    public interface ICacheFactory
    {
        CacheManager GetCacheManager();
        ICacheItemExpiration[] CreateCacheItemExpiration();
    }

    public abstract class BaseCacheFactory
    {
        public abstract string CacheName { get; }

        public virtual CacheManager GetCacheManager() { return CacheFactory.GetCacheManager(CacheName) as CacheManager; }

        public ICacheItemExpiration[] CreateCacheItemExpiration()
        {
            List<ICacheItemExpiration> cacheItemExpiration = new List<ICacheItemExpiration>();

            string slidingSpanName = string.Format(CultureInfo.InvariantCulture, "{0}CacheSlidingExpirationTimeSpan", CacheName);
            string slidingSpan = ConfigurationManager.AppSettings[slidingSpanName];
            if (!string.IsNullOrEmpty(slidingSpan))
            {
                TimeSpan slidingExpirationTimeSpan = TimeSpan.Parse(slidingSpan, CultureInfo.InvariantCulture);
                cacheItemExpiration.Add(new SlidingTime(slidingExpirationTimeSpan));
            }

            string absoluteSpanName = string.Format(CultureInfo.InvariantCulture, "{0}CacheAbsoluteExpirationTimeSpan", CacheName);
            string absoluteSpan = ConfigurationManager.AppSettings[absoluteSpanName];
            if (!string.IsNullOrEmpty(absoluteSpan))
            {
                TimeSpan absoluteExpirationTimeSpan = TimeSpan.Parse(absoluteSpan, CultureInfo.InvariantCulture);
                cacheItemExpiration.Add(new AbsoluteTime(absoluteExpirationTimeSpan));
            }

            return cacheItemExpiration.ToArray();
        }
    }

    public class ConfigCacheFactory : BaseCacheFactory, ICacheFactory
    {
        public override string CacheName { get { return "Config"; } }
    }

    public class LangCacheFactory : BaseCacheFactory, ICacheFactory
    {
        public override string CacheName { get { return "Lang"; } }
    }

    public class ScriptCacheFactory
    {
        public static CacheManager GetCacheManager()
        {
            return CacheFactory.GetCacheManager("Script") as CacheManager;
        }
    }

    public class StyleCacheFactory
    {
        public static CacheManager GetCacheManager()
        {
            return CacheFactory.GetCacheManager("Style") as CacheManager;
        }
    }
}
