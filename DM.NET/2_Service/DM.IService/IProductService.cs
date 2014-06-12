using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM.ViewModel;

namespace DM.IService
{
    public interface IProductService
    {
        ProductViewModel GetProduct(string productID);
    }
}
