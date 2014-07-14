﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace DM.Infrastructure.Cache
{
    public abstract class CacheValue<T>
    {
        private T value;
        public T Value { get { return this.value; } }
        public bool IsDefaultData() { return object.Equals(this.value, default(T)); }

        virtual protected CacheItemPriority GetCachePriority { get { return CacheItemPriority.Normal; } }

        virtual protected void SetValue(T value)
        {
            this.value = value;
        }

        protected abstract T GetFromSource();

        //override this method to do any additional initialization before object is cached.
        virtual protected void Initialize() { return; }
        virtual protected bool ShouldCache() { return true; }
        virtual public bool IsExpired() { return false; }
        virtual public void Remove() { return; }
    }

    public abstract class CacheValueRefID<V, T, CF> : CacheValue<V>
        where T : CacheValueRefID<V, T, CF>, new()
        where CF : ICacheFactory, new()
    {
        public string RefID { get; protected set; }

        public override void Remove()
        {
            ICacheFactory cf = new CF();
            CacheManager cache = cf.GetCacheManager();
            string keyName = MakeKeyName(this.RefID);
            cache.Remove(keyName);
        }

        public static T GetCached(string refID)
        {
            ICacheFactory cf = new CF();
            CacheManager cache = cf.GetCacheManager();
            string keyName = MakeKeyName(refID);
            CacheValueRefID<V, T, CF> item = (CacheValueRefID<V, T, CF>)cache.GetData(keyName);
            if (item != null)
            {
                if (item.IsExpired())
                {
                    item.Remove();
                    item = null;
                }
            }
            return (T)item;
        }

        public static T Get(string refID)
        {
            ICacheFactory cf = new CF();
            CacheManager cache = cf.GetCacheManager();
            string keyName = MakeKeyName(refID);
            CacheValueRefID<V, T, CF> item = (CacheValueRefID<V, T, CF>)cache.GetData(keyName);
            if (item != null)
            {
                if (item.IsExpired())
                {
                    item.Remove();
                    item = null;
                }
            }

            if (item == null)
            {
                item = new T();
                item.RefID = refID;
                item.SetValue(item.GetFromSource());
                item.Initialize();
                if (!item.IsDefaultData() && item.ShouldCache())
                {
                    cache.Add(keyName, item, item.GetCachePriority, null, cf.CreateCacheItemExpiration());
                }
            }
            return (T)item;
        }

        public static T Add(string refID, V value)
        {
            ICacheFactory cf = new CF();
            CacheManager cache = cf.GetCacheManager();
            string keyName = MakeKeyName(refID);
            CacheValueRefID<V, T, CF> item = new T();
            item.RefID = refID;
            item.SetValue(value);
            item.Initialize();
            if (!item.IsDefaultData() && item.ShouldCache())
            {
                cache.Add(keyName, item, item.GetCachePriority, null, cf.CreateCacheItemExpiration());
            }
            return (T)item;
        }

        private static string MakeKeyName(string refID)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", typeof(T).Name, refID);
        }
    }

    public abstract class CacheConfigRefID<T, CF> : CacheValue<XElement>
        where T : CacheConfigRefID<T, CF>, new()
        where CF : ICacheFactory, new()
    {
        public string RefID { get; protected set; }

        protected virtual string ConfigFolder
        {
            get
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                return Path.Combine(baseDir, "App_Data\\Config");
            }
        }
        protected abstract string ConfigSubFolder { get; }

        protected virtual string ConfigFileFormat
        {
            get
            {
                return "{0}.xml";
            }
        }

        protected override XElement GetFromSource()
        {
            string file = string.Format(CultureInfo.InvariantCulture, this.ConfigFileFormat, this.RefID);
            IList<string> paths = new List<string>();
            paths.Add(Path.GetFullPath(Path.Combine(this.ConfigFolder, this.ConfigSubFolder, file)));
            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    return XElement.Load((new XPathDocument(path)).CreateNavigator().ReadSubtree());
                }
            }
            return null;
        }

        public override void Remove()
        {
            ICacheFactory cf = new CF();
            CacheManager cache = cf.GetCacheManager();
            string keyName = MakeKeyName(this.RefID);
            cache.Remove(keyName);
        }

        public static T GetCached(string refID)
        {
            ICacheFactory cf = new CF();
            CacheManager cache = cf.GetCacheManager();
            string keyName = MakeKeyName(refID);
            CacheConfigRefID<T, CF> item = (CacheConfigRefID<T, CF>)cache.GetData(keyName);
            if (item != null)
            {
                if (item.IsExpired())
                {
                    item.Remove();
                    item = null;
                }
            }
            return (T)item;
        }

        public static T Get(string refID)
        {
            ICacheFactory cf = new CF();
            CacheManager cache = cf.GetCacheManager();
            string keyName = MakeKeyName(refID);
            CacheConfigRefID<T, CF> item = (CacheConfigRefID<T, CF>)cache.GetData(keyName);
            if (item != null)
            {
                if (item.IsExpired())
                {
                    item.Remove();
                    item = null;
                }
            }

            if (item == null)
            {
                item = new T();
                item.RefID = refID;
                item.SetValue(item.GetFromSource());
                item.Initialize();
                if (!item.IsDefaultData() && item.ShouldCache())
                {
                    cache.Add(keyName, item, item.GetCachePriority, null, cf.CreateCacheItemExpiration());
                }
            }
            return (T)item;
        }

        public static T Add(string refID, XElement value)
        {
            ICacheFactory cf = new CF();
            CacheManager cache = cf.GetCacheManager();
            string keyName = MakeKeyName(refID);
            CacheConfigRefID<T, CF> item = new T();
            item.RefID = refID;
            item.SetValue(value);
            item.Initialize();
            if (!item.IsDefaultData() && item.ShouldCache())
            {
                cache.Add(keyName, item, item.GetCachePriority, null, cf.CreateCacheItemExpiration());
            }
            return (T)item;
        }

        private static string MakeKeyName(string refID)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", typeof(T).Name, refID);
        }

    }

    

}
