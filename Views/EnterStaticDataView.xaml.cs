using CKL_Studio.Services;
using CKL_Studio.ViewModels;
using System.Windows.Controls;

namespace CKL_Studio.EnterStaticData
{
    /// <summary>
    /// Логика взаимодействия для EnterStaticDataView.xaml
    /// </summary>
    public partial class EnterStaticDataView : UserControl
    {
        CKLService _service;
        public EnterStaticDataView()
        {
            InitializeComponent();

            this.DataContext = new EnterStaticDataVM(_service);

        }
    }
}
