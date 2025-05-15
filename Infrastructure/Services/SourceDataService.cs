using CKL_Studio.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKLLib;
using System.Printing;
using System.Runtime.InteropServices.Marshalling;

namespace CKL_Studio.Infrastructure.Services
{
    public class SourceDataService : IDataService<Pair>
    {
        private HashSet<Pair> _pairs = new HashSet<Pair>();

        public void Add(Pair item)
        {
            if (item != null) 
                _pairs.Add(item);

            throw new ArgumentNullException(nameof(item));
        }

        public void Delete(Pair item)
        {
            if (item != null)
                _pairs.Remove(item);

            throw new ArgumentNullException(nameof(item));
        }

        public Pair? Get(string identifier)
        {
            return _pairs.FirstOrDefault(p => p.ToString() == identifier);
        }

        public IEnumerable<Pair> GetAll()
        {
            return _pairs;
        }

        public void Update(Pair item)
        {
            throw new NotImplementedException();
        }
    }
}
