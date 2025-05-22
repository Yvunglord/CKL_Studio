using CKLLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Common.Interfaces.Factories
{
    public interface ISolutionExplorerDataServiceFactory
    {
        IDataService<CKL> Create(CKL originalCkl);
    }
}
