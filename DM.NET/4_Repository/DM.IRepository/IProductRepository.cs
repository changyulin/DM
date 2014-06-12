using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM.Infrastructure.Repository;
using DM.DomainModel;


namespace DM.IRepository
{
    public interface IProductRepository
    {
        OperateResult AddReturn(Product item);
        OperateResult RemoveReturn(Product item);
        OperateResult UpdateReturn(Product item);
        Product FindById(string productId);
    }
}
