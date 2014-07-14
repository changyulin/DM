using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.Infrastructure.Cache
{
    public class ConfigListItem : CacheConfigRefID<ConfigListItem, ConfigCacheFactory>
    {
        protected override string ConfigSubFolder { get { return "ConfigList"; } }
    }
}
