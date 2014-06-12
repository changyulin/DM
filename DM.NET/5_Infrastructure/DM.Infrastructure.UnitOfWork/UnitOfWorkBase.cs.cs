using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM.Infrastructure.Domain;
using DM.Infrastructure.Repository;

namespace DM.Infrastructure.UnitOfWork
{
    public abstract class UnitOfWorkBase<T> : IUnitOfWorkRepository, IRepository<T>
        where T : EntityBase
    {
        private IUnitOfWork unitOfWork;
        public UnitOfWorkBase() { }
        public UnitOfWorkBase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void Add(T item)
        {
            this.unitOfWork.RegisterAdded(item, this);
        }

        public void Remove(T item)
        {
            this.unitOfWork.RegisterDeleted(item, this);
        }

        public void Update(T item)
        {
            this.unitOfWork.RegisterUpdated(item, this);
        }

        public abstract void PersistCreationOf(EntityBase entity);

        public abstract void PersistUpdateOf(EntityBase entity);

        public abstract void PersistDeletionOf(EntityBase entity);
    }
}
