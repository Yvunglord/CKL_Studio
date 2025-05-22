using CKL_Studio.Common.Interfaces;
using CKL_Studio.Common.Interfaces.CKLInterfaces;
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
    public class CKLOperationService : ICklOperationService
    {
        private readonly IDialogService _dialogService;
        private readonly CKL _currentCkl;
        private readonly Action<CKL> _resetCkl;

        public CKLOperationService(IDialogService dialogService, CKL currentCkl, Action<CKL> resetCkl)
        {
            _dialogService = dialogService;
            _currentCkl = currentCkl;
            _resetCkl = resetCkl;
        }

        public void ExecuteBinaryOperation(Func<CKL, CKL, CKL> operation, string filter, string defaultDir)
        {
            var path = _dialogService.ShowOpenFileDialog(filter, defaultDir);
            if (path != null) 
            {
                var ckl = CKL.GetFromFile(path);
                if (ckl != null) 
                {
                    try
                    {
                        var result = operation(_currentCkl, ckl);
                        _resetCkl(result);
                    }
                    catch (ArgumentException ex)
                    {
                        _dialogService.ShowMessage($"Uncorrect data: {ex.Message}", "Error");
                    }
                }
            }
        }

        public void ExecuteOperation(Func<CKL, CKL> operation)
        {
            try
            {
                var result = operation(_currentCkl);
                _resetCkl(result);
            }
            catch (ArgumentException ex)
            {
                _dialogService.ShowMessage($"Uncorrect data: {ex.Message}", "Error");
            }
        }

        public void ExecuteParameterizedTimeOperation(Func<CKL, TimeInterval, double, CKL> operation, string dialogTitle)
        {
            var dialog = new ParameterizedTimeOperationDialog() { Title = dialogTitle};
            if (_dialogService.ShowDialog<ParameterizedTimeOperationDialog>(d => d.Title = dialogTitle) == true)
            {
                var stTime = double.Parse(dialog.TextBox1Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                var enTime = double.Parse(dialog.TextBox2Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                var t = double.Parse(dialog.TextBox3Value, NumberStyles.Any, CultureInfo.InvariantCulture);

                try
                {
                    var result = operation(_currentCkl, new TimeInterval(stTime, enTime), t);
                    _resetCkl(result);
                }
                catch (ArgumentException ex)
                {
                    _dialogService.ShowMessage($"Uncorrect data: {ex.Message}", "Error");
                } 
            } 
        }

        public void ExecuteTimeOperation(Func<CKL, TimeInterval, CKL> operation, string dialogTitle)
        {
            var dialog = new TimeOperationDialog() { Title = dialogTitle };
            if (_dialogService.ShowDialog<TimeOperationDialog>(d => d.Title = dialogTitle) == true)
            {
                var stTime = double.Parse(dialog.TextBox1Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                var enTime = double.Parse(dialog.TextBox2Value, NumberStyles.Any, CultureInfo.InvariantCulture);

                try
                {
                    var result = operation(_currentCkl, new TimeInterval(stTime, enTime));
                    _resetCkl(result);
                }
                catch (ArgumentException ex)
                {
                    _dialogService.ShowMessage($"Uncorrect data: {ex.Message}", "Error");
                }
            } 
        }
    }
}
