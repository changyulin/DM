using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM.Infrastructure.UnitOfWork;
using DM.IRepository;
using DM.Infrastructure.Repository;
using DM.Infrastructure.Domain;

namespace DM.Repository
{
    public class ProductRepository2 : IUnitOfWorkRepository, IRepository<DomainModel.Product>
    {
        private IUnitOfWork unitOfWork;
        public ProductRepository2(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



        public DomainModel.Product FindById(string productId)
        {
            throw new NotImplementedException();
        }

        public void PersistCreationOf(EntityBase entity)
        {
            //ADO.NET code or Linq code
            throw new NotImplementedException();
        }

        public void PersistUpdateOf(EntityBase entity)
        {
            //ADO.NET code or Linq code
            throw new NotImplementedException();
        }

        public void PersistDeletionOf(EntityBase entity)
        {
            //ADO.NET code or Linq code
            throw new NotImplementedException();
        }

        public void Add(DomainModel.Product item)
        {
            this.unitOfWork.RegisterAdded(item, this);
        }

        public void Remove(DomainModel.Product item)
        {
            this.unitOfWork.RegisterDeleted(item, this);
        }

        public void Update(DomainModel.Product item)
        {
            this.unitOfWork.RegisterUpdated(item, this);
        }
    }
}
