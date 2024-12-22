using CKL_Studio.Services;
using CKL_Studio.ViewModels;
using System.Windows.Controls;

namespace CKL_Studio.Views
{
    /// <summary>
    /// Логика взаимодействия для MakeRelationView.xaml
    /// </summary>
    public partial class MakeRelationView : UserControl
    {
        CKLService _service;

        public MakeRelationView()
        {
            InitializeComponent();

            this.DataContext = new MakeRelationVM(_service);
        }
    }
}
