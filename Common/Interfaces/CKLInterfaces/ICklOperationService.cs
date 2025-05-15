using CKLLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Common.Interfaces.CKLInterfaces
{
    public interface ICklOperationService
    {
        void ExecuteOperation(Func<CKL, CKL> operation);
        void ExecuteBinaryOperation(Func<CKL, CKL, CKL> operation, string filter, string defaultDir);
        void ExecuteTimeOperation(Func<CKL, TimeInterval, CKL> operation, string dialogTitle);
        void ExecuteParameterizedTimeOperation(Func<CKL, TimeInterval, double, CKL> operation, string dialogTitle);
    }
}
