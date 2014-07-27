using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Infrastructure.Cache
{
    public interface ICache
    {
        int Count { get; }
        void Add(string key, object value);
        T Get<T>(string key);
        bool Contains(string key);
        void Remove(string key);
        void Clear();
    }
}
