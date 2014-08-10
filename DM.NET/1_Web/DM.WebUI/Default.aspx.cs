using System;
using System.Xml.Linq;
using DM.Infrastructure.Cache;
using DM.Infrastructure.Log;
using DM.Infrastructure.Util.CryptologyHelpers;
using DM.Infrastructure.Util.LangHelpers;
using DM.IService;
using DM.ViewModel;
using StackExchange.Profiling;
using StructureMap;

namespace DM.WebUI
{
    public partial class _Default : BasePage
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

            using (MiniProfiler.Current.Step("Page_Load()"))
            {
                ProductViewModel product = productService.GetProduct("1");
                LogHelper.Debug("log Debug!");
            }
        }
    }
}
