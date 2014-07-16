using System;
using System.Web.UI;
using System.Xml.Linq;
using DM.Infrastructure.Cache;
using DM.Infrastructure.Log;
using DM.Infrastructure.Util.LangHelpers;
using DM.Infrastructure.Util.ScriptHelpers;
using DM.IService;
using DM.ViewModel;
using StackExchange.Profiling;
using StructureMap;

namespace DM.WebUI
{
    public partial class _Default : Page
    {
        private IProductService productService;

        public _Default()
        {
            productService = ObjectFactory.GetNamedInstance<IProductService>("Default");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            XElement configTest = ConfigListItem.Get("ConfigTest").Value;
            string home = LangHelpers.GetString("homeLabel", "UI");
            ScriptHelpers.LoadScript(this.Page,"Script");
            using (MiniProfiler.Current.Step("Page_Load()"))
            {
                ProductViewModel product = productService.GetProduct("1");
                LogHelper.Debug("log Debug!");
            }
        }
    }
}
