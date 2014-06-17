using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;

namespace DM.Infrastructure.Route
{
    public class RoutesConfig : IConfigurationSectionHandler
    {
        public static RouteRuleCollection routeRuleCollection;

        public static void Configure(string sectionName)
        {
            ConfigurationManager.GetSection(sectionName);
        }

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            XElement x = XElement.Parse(section.CreateNavigator().OuterXml);
            List<RouteRule> routes = x.Elements().Select(r => new RouteRule
            {
                UrlRule = r.Attribute("urlRule").Value,
                RedirectTo = r.Attribute("redirectTo").Value,
                RouteHandler = r.Attribute("routeHandler") == null ? "Default" : r.Attribute("routeHandler").Value
            }).ToList();
            routeRuleCollection = new RouteRuleCollection(routes);
            return routeRuleCollection;
        }
    }
}
