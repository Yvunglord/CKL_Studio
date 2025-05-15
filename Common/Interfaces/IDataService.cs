using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Common.Interfaces
{
    public interface IDataService<T>
    {
        T? Get(string identifier);
        IEnumerable<T> GetAll();
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
