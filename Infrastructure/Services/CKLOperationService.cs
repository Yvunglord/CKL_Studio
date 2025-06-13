using CKL_Studio.Common.Interfaces;
using CKL_Studio.Common.Interfaces.CKLInterfaces;
using CKL_Studio.Presentation.ViewModels.Dialog;
using CKL_Studio.Presentation.Windows.Dialogs;
using CKLLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Infrastructure.Services
{
    public class CklOperationService : ICklOperationService
    {
        public void ExecuteBinaryOperation(Func<CKL, CKL, CKL> operation, CKL current, IEnumerable<CKL> related)
        {
            var dialog = new SelectCklDialogViewModel(related, current.FilePath);
            dialog.RequestClose += result =>
            {
                if (result && dialog.SelectedCkl != null)
                {
                    var resultCkl = operation(current, dialog.SelectedCkl);
                    CKL.Save(resultCkl);
                }
            };
        }

        public void ExecuteOperation(Func<CKL, CKL> operation)
        {
            throw new NotImplementedException();
        }

        public void ExecuteParameterizedTimeOperation(Func<CKL, TimeInterval, double, CKL> operation)
        {
            throw new NotImplementedException();
        }

        public void ExecuteTimeOperation(Func<CKL, TimeInterval, CKL> operation)
        {
            throw new NotImplementedException();
        }
    }
}
