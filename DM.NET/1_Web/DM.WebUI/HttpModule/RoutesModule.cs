using System;
using System.Text.RegularExpressions;
using System.Web;
using DM.Infrastructure.Route;
using StructureMap;

namespace DM.WebUI.HttpModule
{
    public class RoutesModule : IHttpModule
    {
        /// <summary>
        /// Rewriter url if need
        /// </summary>
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.CurrentHandler == null && string.IsNullOrEmpty(HttpContext.Current.Request.CurrentExecutionFilePathExtension))
            {
                RouteRuleCollection rules = RoutesConfig.routeRuleCollection;
                for (int i = 0; i < rules.Count; i++)
                {
                    if (Regex.IsMatch(HttpContext.Current.Request.Path, rules[i].UrlRule, RegexOptions.IgnoreCase))
                    {
                        IUrlRewriter urlRewriter = ObjectFactory.GetNamedInstance<IUrlRewriter>(rules[i].RouteHandler);
                        urlRewriter.Rewrite(HttpContext.Current, rules[i].RedirectTo);
                        break;
                    }
                }
            }
        }

        public void Dispose() { }
        #endregion
    }
}
