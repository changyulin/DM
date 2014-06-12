using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM.Infrastructure.Domain;

namespace DM.Infrastructure.Repository.IRepository
{
    public interface IReadOnlyRepository<T> where T : EntityBase
    {
        T FindBy(object id);
        IEnumerable<T> FindAll();
        //IEnumerable<T> FindBy(Query query);
        //IEnumerable<T> FindBy(Query query, int index, int count);
    }
}
