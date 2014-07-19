using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Infrastructure.Cache
{
    public class FeatureItem : CacheConfigRefID<FeatureItem, ConfigCacheFactory>
    {
        protected override string ConfigSubFolder { get { return "Feature"; } }
    }
}
