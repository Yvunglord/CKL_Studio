using CKL_Studio.Common.Interfaces;
using CKLLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Infrastructure.Services
{
    public class SolutionExplorerDataService : IDataService<CKL>
    {
        private readonly CKL _originalCkl;

        public SolutionExplorerDataService(CKL originalCkl)
        {
            _originalCkl = originalCkl;
        }

        public void Add(CKL item)
        {
            if (item.FilePath.)
        }

        public void Delete(CKL item)
        {
            throw new NotImplementedException();
        }

        public CKL? Get(string identifier)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CKL> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(CKL item)
        {
            throw new NotImplementedException();
        }

        public void Save()
        { 
            
        }
    }
}
