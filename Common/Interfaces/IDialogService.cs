using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CKL_Studio.Common.Interfaces
{
    public interface IDialogService
    {
        bool? ShowConfirmationDialog(string title, string message);
        bool? ShowDialog<TDialog>(Action<TDialog>? setup = null) where TDialog : Window, new();
        string? ShowOpenFileDialog(string filter, string defaultDir);
        string? ShowSaveFileDialog(string defaultName, string defaultDir, string filter);
        void ShowMessage(string message, string caption = "Сообщение");
    }
}
