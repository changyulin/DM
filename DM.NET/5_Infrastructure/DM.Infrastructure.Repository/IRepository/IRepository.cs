using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM.Infrastructure.Domain;

namespace DM.Infrastructure.Repository
{
    public interface IRepository<T> where T : EntityBase
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);
    }
}
