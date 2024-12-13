﻿using CKL_Studio.MVVM.ViewModel;
using Microsoft.Win32;
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

namespace CKL_Studio.OnLoad
{
    /// <summary>
    /// Логика взаимодействия для OnLoadView.xaml
    /// </summary>
    public partial class OnLoadView : UserControl
    {
        public OnLoadView()
        {
            InitializeComponent();

            this.DataContext = new OnLoadViewModel();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigateToDataEntryControl();
        }

        private void NavigateToDataEntryControl()
        {
            var viewModel = new OnLoadViewModel();

            var preprocessingWindow = Window.GetWindow(this) as PreprocessingWindow;

            if (preprocessingWindow != null)
            {
                //preprocessingWindow.PreprocessingContentControl.Content = new EnterStaticData.EnterStaticDataView();
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                string filePath = openFileDialog.FileName;
                DateTime lastChange = System.IO.File.GetLastWriteTime(filePath);

                // Добавление файла в ViewModel
                var viewModel = DataContext as OnLoadViewModel;
                viewModel?.AddFile(fileName, filePath, lastChange);

                viewModel?.SaveFiles();
            }
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = DataContext as OnLoadViewModel;
                viewModel?.AddSearchHistoryItem();
                viewModel?.SaveSearchHistory();
            }
        }
    }
}
