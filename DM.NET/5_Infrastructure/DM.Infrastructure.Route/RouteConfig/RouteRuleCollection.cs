using System.Collections.Generic;
using System.Linq;

namespace DM.Infrastructure.Route
{
    public class RouteRuleCollection
    {
        private readonly List<RouteRule> routes;

        public RouteRuleCollection(List<RouteRule> routes)
        {
            for (int i = 0; i < routes.Count; i++)
            {
                if (string.IsNullOrEmpty(routes[i].UrlRule) || string.IsNullOrEmpty(routes[i].RedirectTo) || string.IsNullOrEmpty(routes[i].RouteHandler))
                {
                    routes.Remove(routes[i]);
                    i--;
                }
            }
            this.routes = routes;
        }

        public int Count
        {
            get { return routes.Count(); }
        }

        public RouteRule this[int index]
        {
            get { return routes[index]; }
        }
    }
}
