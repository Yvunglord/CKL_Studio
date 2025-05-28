using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для SourceInputView.xaml
    /// </summary>
    public partial class SourceInputView : UserControl
    {
        public SourceInputView()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;
                var binding = textBox.GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();

                Keyboard.ClearFocus();
                e.Handled = true;
            }
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var focusedElement = Keyboard.FocusedElement as TextBox;

            if (focusedElement != null && focusedElement.Name == "RowCountTextBox") 
            {
                var binding = focusedElement.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource(); 
                Keyboard.ClearFocus();  
            }
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            var currentListBox = sender as ListBox;
            if (currentListBox == null || currentListBox.Items.Count == 0)
                return;

            int currentIndex = currentListBox.SelectedIndex;
            var listBoxes = new List<ListBox>();

            var parent = VisualTreeHelper.GetParent(currentListBox);
            while (parent != null && !(parent is ScrollViewer))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is ScrollViewer scrollViewer)
            {
                var stackPanel = scrollViewer.Content as StackPanel;
                if (stackPanel != null)
                {
                    foreach (var child in stackPanel.Children)
                    {
                        if (child is ListBox lb && lb.Visibility == Visibility.Visible)
                        {
                            listBoxes.Add(lb);
                        }
                    }
                }
            }

            int currentListBoxIndex = listBoxes.IndexOf(currentListBox);

            switch (e.Key)
            {
                case Key.Up:
                    if (currentIndex > 0)
                    {
                        currentListBox.SelectedIndex = currentIndex - 1;
                        currentListBox.ScrollIntoView(currentListBox.SelectedItem);
                    }
                    e.Handled = true;
                    break;

                case Key.Down:
                    if (currentIndex < currentListBox.Items.Count - 1)
                    {
                        currentListBox.SelectedIndex = currentIndex + 1;
                        currentListBox.ScrollIntoView(currentListBox.SelectedItem);
                    }
                    e.Handled = true;
                    break;

                case Key.Left:
                    if (currentListBoxIndex > 0)
                    {
                        var prevListBox = listBoxes[currentListBoxIndex - 1];
                        prevListBox.SelectedIndex = Math.Min(currentIndex, prevListBox.Items.Count - 1);
                        prevListBox.Focus();
                        prevListBox.ScrollIntoView(prevListBox.SelectedItem);
                    }
                    e.Handled = true;
                    break;

                case Key.Right:
                    if (currentListBoxIndex < listBoxes.Count - 1)
                    {
                        var nextListBox = listBoxes[currentListBoxIndex + 1];
                        nextListBox.SelectedIndex = Math.Min(currentIndex, nextListBox.Items.Count - 1);
                        nextListBox.Focus();
                        nextListBox.ScrollIntoView(nextListBox.SelectedItem);
                    }
                    e.Handled = true;
                    break;
            }
        }
    }
}
