using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DM.Infrastructure.Route
{
    public class DefaultUrlRewriter : IUrlRewriter
    {
        public void Rewrite(HttpContext context, string redirectToUrl)
        {
            string queryString = string.IsNullOrEmpty(context.Request.Url.Query) ? "" : context.Request.Url.Query.Substring(1);
            context.RewritePath(redirectToUrl, string.Empty, queryString, false);
        }
    }
}
