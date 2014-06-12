using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM.IRepository;
using DM.Infrastructure.Repository;
using DM.DomainModel;
using DM.Infrastructure.UnitOfWork;

namespace DM.Repository
{
    public class ProductRepository : UnitOfWorkBase<DomainModel.Product>, IProductRepository
    {
        public ProductRepository() { }
        public ProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override void PersistCreationOf(Infrastructure.Domain.EntityBase entity)
        {
            //ADO.NET code or Linq code
            throw new NotImplementedException();
        }

        public override void PersistUpdateOf(Infrastructure.Domain.EntityBase entity)
        {
            //ADO.NET code or Linq code
            throw new NotImplementedException();
        }

        public override void PersistDeletionOf(Infrastructure.Domain.EntityBase entity)
        {
            //ADO.NET code or Linq code
            throw new NotImplementedException();
        }

        public OperateResult AddReturn(DomainModel.Product item)
        {
            //ADO.NET code or Linq code
            throw new NotImplementedException();
        }

        public OperateResult RemoveReturn(DomainModel.Product item)
        {
            //ADO.NET code or Linq code
            throw new NotImplementedException();
        }

        public OperateResult UpdateReturn(DomainModel.Product item)
        {
            //ADO.NET code or Linq code
            throw new NotImplementedException();
        }



        public DomainModel.Product FindById(string productId)
        {
            using (ShopDBDataContext db = new ShopDBDataContext())
            {
                var product = (from o in db.Product
                               where o.ProductID == productId
                               select new DomainModel.Product
                               {
                                   ProductID = o.ProductID,
                                   BrandID = o.BrandID,
                                   Brand = o.Brands.BrandName,
                                   CategoryID = o.CategoryID,
                                   Category = o.Category.CategoryName,
                                   ProductName = o.ProductName,
                                   ProductColor = o.ProductColor,
                                   ProductPrice = o.ProductPrice,
                                   ProductSize = o.ProductSize
                               }).FirstOrDefault();
                return product;
            }
        }




    }
}
