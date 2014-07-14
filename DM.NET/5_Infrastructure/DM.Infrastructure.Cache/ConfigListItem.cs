
namespace DM.Infrastructure.Cache
{
    public class ConfigListItem : CacheConfigRefID<ConfigListItem, ConfigCacheFactory>
    {
        protected override string ConfigSubFolder { get { return "ConfigList"; } }
    }
}
