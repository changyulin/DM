using DM.Infrastructure.Domain;

namespace DM.Infrastructure.UnitOfWork
{
    public interface IUnitOfWorkRepository
    {
        void PersistCreationOf(EntityBase entity);
        void PersistUpdateOf(EntityBase entity);
        void PersistDeletionOf(EntityBase entity);
    }
}
