using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using StructureMap;

namespace DM.Infrastructure.Cache
{
    public interface ICacheFactory
    {
        ICache GetCacheManager();
    }

    public class ConfigCacheFactory :  ICacheFactory
    {
        public ICache GetCacheManager()
        {
            return ObjectFactory.GetNamedInstance<ICache>("Config");
        }
    }

    public class LangCacheFactory :  ICacheFactory
    {
        public ICache GetCacheManager()
        {
            return ObjectFactory.GetNamedInstance<ICache>("Lang");
        }
    }

    public class ScriptCacheFactory : ICacheFactory
    {
        public ICache GetCacheManager()
        {
            return ObjectFactory.GetNamedInstance<ICache>("Script");
        }
    }

    public class StyleCacheFactory : ICacheFactory
    {
        public ICache GetCacheManager()
        {
            return ObjectFactory.GetNamedInstance<ICache>("Style");
        }
    }

    public class CacheFactory
    {
        public static ICache CreateConfigCache()
        {
            return ObjectFactory.GetNamedInstance<ICache>("Config");
        }

        public static ICache CreateLangCache()
        {
            return ObjectFactory.GetNamedInstance<ICache>("Lang");
        }

        public static ICache CreateScriptCache()
        {
            return ObjectFactory.GetNamedInstance<ICache>("Script");
        }

        public static ICache CreateStyleCache()
        {
            return ObjectFactory.GetNamedInstance<ICache>("Style");
        }
    }
}
