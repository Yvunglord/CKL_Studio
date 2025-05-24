using CKL_Studio.Presentation.ViewModels.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CKL_Studio.Presentation.Windows.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SelectCklDialog.xaml
    /// </summary>
    public partial class SelectCklDialog : Window
    {
        public SelectCklDialog()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is SelectCklDialogViewModel vm)
            {
                vm.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(SelectCklDialogViewModel.DialogResult))
                    {
                        DialogResult = vm.DialogResult;
                    }
                };
            }
        }
    }
}
