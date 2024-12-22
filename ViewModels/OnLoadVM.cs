using CKL_Studio.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CKL_Studio.ViewModels
{
    public class OnLoadVM : INotifyPropertyChanged
    {
        private ObservableCollection<FileModel> _allFiles;
        private ObservableCollection<FileModel> _filteredFiles;
        private ObservableCollection<string> _searchHistory;
        private string _searchText;

        public ObservableCollection<FileModel> FilteredFiles
        {
            get => _filteredFiles;
            set
            {
                _filteredFiles = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SearchHistory
        {
            get => _searchHistory;
            set
            {
                _searchHistory = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterFiles();
            }
        }

        public OnLoadVM()
        {
            _allFiles = new ObservableCollection<FileModel>();
            FilteredFiles = new ObservableCollection<FileModel>();
            SearchHistory = new ObservableCollection<string>();

            LoadSearchHistory();
            LoadFiles();

            Application.Current.Exit += (s, e) =>
            {
                SaveSearchHistory();
                SaveFiles();
            };
        }

        private void FilterFiles()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredFiles = new ObservableCollection<FileModel>(_allFiles);
            }
            else
            {
                FilteredFiles = new ObservableCollection<FileModel>(
                    _allFiles.Where(file => file.FileName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            }
        }

        private void UpdateSearchHistory()
        {
            if (!string.IsNullOrWhiteSpace(SearchText) && !SearchHistory.Contains(SearchText))
            {
                SearchHistory.Insert(0, SearchText);
                if (SearchHistory.Count > 5)
                {
                    SearchHistory.RemoveAt(5);
                }
            }
        }

        private void LoadSearchHistory()
        {
            try
            {
                if (File.Exists("search_history.txt"))
                {
                    var lines = File.ReadAllLines("search_history.txt");
                    foreach (var line in lines)
                    {
                        SearchHistory.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории поиска: {ex.Message}");
            }
        }

        public void SaveSearchHistory()
        {
            try
            {
                File.WriteAllLines("search_history.txt", SearchHistory);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения истории поиска: {ex.Message}");
            }
        }

        private void LoadFiles()
        {
            try
            {
                if (File.Exists("files.txt"))
                {
                    var lines = File.ReadAllLines("files.txt");
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 3)
                        {
                            var fileName = parts[0];
                            var filePath = parts[1];
                            var lastChange = DateTime.Parse(parts[2]);
                            AddFile(fileName, filePath, lastChange);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки списка файлов: {ex.Message}");
            }
        }

        public void SaveFiles()
        {
            try
            {
                var lines = _allFiles.Select(file => $"{file.FileName}|{file.FilePath}|{file.LastChange}");
                File.WriteAllLines("files.txt", lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения списка файлов: {ex.Message}");
            }
        }

        public void AddFile(string fileName, string filePath, DateTime lastChange)
        {
            if (!_allFiles.Any(file => file.FilePath == filePath))
            {
                var newFile = new FileModel(fileName, filePath, lastChange);
                _allFiles.Add(newFile);
                FilteredFiles.Add(newFile);
            }
        }

        public void AddSearchHistoryItem()
        {
            UpdateSearchHistory();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
