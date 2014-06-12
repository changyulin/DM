using System.Collections.Generic;
using System.Transactions;
using DM.Infrastructure.Domain;

namespace DM.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private Dictionary<EntityBase, IUnitOfWorkRepository> addedEntities;
        private Dictionary<EntityBase, IUnitOfWorkRepository> updatedEntities;
        private Dictionary<EntityBase, IUnitOfWorkRepository> deletedEntities;

        public UnitOfWork()
        {
            addedEntities = new Dictionary<EntityBase, IUnitOfWorkRepository>();
            updatedEntities = new Dictionary<EntityBase, IUnitOfWorkRepository>();
            deletedEntities = new Dictionary<EntityBase, IUnitOfWorkRepository>();
        }

        public void RegisterAdded(EntityBase entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            addedEntities.Add(entity, unitofWorkRepository);
        }

        public void RegisterUpdated(EntityBase entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            updatedEntities.Add(entity, unitofWorkRepository);
        }

        public void RegisterDeleted(EntityBase entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            deletedEntities.Add(entity, unitofWorkRepository);
        }

        public void Commit()
        {
            using (TransactionScope scope=new TransactionScope())
            {
                foreach (var entity in deletedEntities.Keys)
                {
                    this.deletedEntities[entity].PersistDeletionOf(entity);
                }

                foreach (var entity in addedEntities.Keys)
                {
                    this.addedEntities[entity].PersistCreationOf(entity);
                }

                foreach (var entity in updatedEntities.Keys)
                {
                    this.updatedEntities[entity].PersistUpdateOf(entity);
                }

                scope.Complete();
            }
        }
    }
}
