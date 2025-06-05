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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CKL_Studio.Presentation.Views
{
    /// <summary>
    /// Логика взаимодействия для CKLModelView.xaml
    /// </summary>
    public partial class CKLModelView : UserControl
    {
        public CKLModelView()
        {
            InitializeComponent();
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var listBox in FindVisualChildren<ListBox>(this))
            {
                if (listBox.IsKeyboardFocusWithin)
                {
                    Keyboard.ClearFocus();
                    break;
                }
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t)
                    yield return t;

                foreach (var nestedChild in FindVisualChildren<T>(child))
                    yield return nestedChild;
            }
        }
    }
}
