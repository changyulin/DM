using System.Web;

namespace DM.Infrastructure.Route
{
    public interface IUrlRewriter
    {
        void Rewrite(HttpContext context, string redirectToUrl);
    }
}
