using StructureMap;

namespace DM.Infrastructure.Log
{
    public class LogFactory
    {
        public static ILogger GetLogger()
        {
            return ObjectFactory.GetNamedInstance<ILogger>("Default");
        }
    }
}
