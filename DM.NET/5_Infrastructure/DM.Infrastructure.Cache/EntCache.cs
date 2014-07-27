using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace DM.Infrastructure.Cache
{
    /// <summary>
    /// Enterprise Library Cache Adapter
    /// </summary>
    public class EntCache : ICache
    {
        private string CacheName;
        private CacheManager cacheManager;

        public EntCache(string cacheName)
        {
            CacheName = cacheName;
            cacheManager = Microsoft.Practices.EnterpriseLibrary.Caching.CacheFactory.GetCacheManager(cacheName) as CacheManager;
        }

        public int Count
        {
            get { return cacheManager.Count; }
        }

        public void Add(string key, object value)
        {
            cacheManager.Add(key, value, CacheItemPriority.Normal, null, this.CreateCacheItemExpiration());
        }

        public T Get<T>(string key)
        {
            T item = (T)cacheManager.GetData(key);
            if (null == item)
                item = default(T);
            return item;
        }

        public bool Contains(string key)
        {
            return cacheManager.Contains(key);
        }

        public void Remove(string key)
        {
            cacheManager.Remove(key);
        }

        public void Clear()
        {
            cacheManager.Flush();
        }

        protected virtual ICacheItemExpiration[] CreateCacheItemExpiration()
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

    public class ConfigEntCache : EntCache
    {
        public ConfigEntCache() : base("Config") { }
    }

    public class LangEntCache : EntCache
    {
        public LangEntCache() : base("Lang") { }
    }

    public class ScriptEntCache : EntCache
    {
        public ScriptEntCache() : base("Script") { }
    }

    public class StyleEntCache : EntCache
    {
        public StyleEntCache() : base("Style") { }
    }
}
