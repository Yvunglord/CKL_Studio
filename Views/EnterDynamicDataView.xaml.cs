using CKL_Studio.Services;
using CKL_Studio.ViewModels;
using System.Windows.Controls;
namespace CKL_Studio.EnterDynamicData
{
    /// <summary>
    /// Логика взаимодействия для EnterDynamicDataView.xaml
    /// </summary>
    public partial class EnterDynamicDataView : UserControl
    {
        CKLService _service;
        public EnterDynamicDataView()
        {
            InitializeComponent();

            this.DataContext = new EnterDynamicDataVM(_service);
        }
    }
}
