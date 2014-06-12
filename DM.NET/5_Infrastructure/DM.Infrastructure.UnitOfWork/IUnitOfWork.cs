using DM.Infrastructure.Domain;

namespace DM.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        void RegisterAdded(EntityBase entity, IUnitOfWorkRepository unitofWorkRepository);
        void RegisterUpdated(EntityBase entity, IUnitOfWorkRepository unitofWorkRepository);
        void RegisterDeleted(EntityBase entity, IUnitOfWorkRepository unitofWorkRepository);
        void Commit();
    }
}
