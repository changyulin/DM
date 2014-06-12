using System.IO;

namespace DM.Infrastructure.Util.StructureMapHelpers
{
    public static class StructureMapHelper
    {
        /// <summary>
        /// Initialize StructureMap config using the given config file
        /// </summary>
        /// <param name="filename">Config file name</param>
        public static void Initialize(string filename)
        {
            if (File.Exists(filename))
            {
                StructureMap.ObjectFactory.Initialize(x =>
                {
                    x.AddConfigurationFromXmlFile(filename);
                });
            }
        }

        /// <summary>
        /// Add or override StructureMap config using the given config file
        /// </summary>
        /// <param name="filename">Config file name</param>
        public static void Configure(string filename)
        {
            if (File.Exists(filename))
            {
                StructureMap.ObjectFactory.Configure(x =>
                {
                    x.AddConfigurationFromXmlFile(filename);
                });
            }
        }
    }
}
