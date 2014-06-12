using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DM.IService;
using DM.Service;
using DM.ViewModel;
using StackExchange.Profiling;
using DM.Infrastructure.Log;
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
            using (MiniProfiler.Current.Step("Page_Load()"))
            { 
                ProductViewModel product = productService.GetProduct("1");
                LogHelper.Debug("log Debug!");
            }
        }
    }
}
