using CKL_Studio.Common.Interfaces;
using CKL_Studio.Common.Interfaces.Factories;
using CKLLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Infrastructure.Services.Factories
{
    public class SolutionExplorerDataServiceFactory : ISolutionExplorerDataServiceFactory
    {
        public IDataService<CKL> Create(CKL originalCkl)
        {
            return new SolutionExplorerDataService(originalCkl);
        }
    }
}
