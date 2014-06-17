namespace DM.Infrastructure.Route
{
    public class RouteRule
    {
        private string urlRule;
        private string redirectTo;
        private string routeHandler;

        public string UrlRule
        {
            get { return urlRule; }
            set { urlRule = value; }
        }

        public string RedirectTo
        {
            get { return redirectTo; }
            set { redirectTo = value; }
        }

        public string RouteHandler
        {
            get { return routeHandler; }
            set { routeHandler = value; }
        }
    }
}
