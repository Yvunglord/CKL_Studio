using CKL_Studio.Common.Interfaces;
using CKL_Studio.Presentation.ViewModels.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Infrastructure.Services
{
    public class FileDataService : IDataService<FileData>
    {
        private readonly ObservableCollection<FileData> _allFiles = new ObservableCollection<FileData>();
        private ObservableCollection<FileData> _filteredFiles = new ObservableCollection<FileData>();
        private const string _storagePath = "recent_files.json";

        public ObservableCollection<FileData> FilteredFiles => _filteredFiles;

        public void Add(FileData item)
        {
            if (!_allFiles.Any(f => f.Path == item.Path))
                _allFiles.Add(item);
        }

        public void Delete(FileData item)
        {
            _allFiles.Remove(item);
            _filteredFiles.Remove(item);
        }

        public FileData? Get(string identifier)
        {
            var file = _allFiles.FirstOrDefault(f => f.Path == identifier);
            if (file != null) 
                return file;

            return null;
        }

        public IEnumerable<FileData> GetAll()
        {
           return _allFiles;
        }

        public void Update(FileData item)
        {
            var existing = Get(item.Path);
            if (existing != null)
            { 
                existing.Name = item.Name;
                existing.LastAccess = item.LastAccess;
                existing.Path = item.Path;
            }
        }

        public void Load()
        {
            if (File.Exists(_storagePath))
            { 
                var json = File.ReadAllText(_storagePath);
                var files = JsonConvert.DeserializeObject<List<FileData>>(json) ?? new List<FileData>();

                _allFiles.Clear();
                foreach (var file in files)
                {
                    _allFiles.Add(file);
                }

                FilterFiles(string.Empty);
            }
        }

        public void Save() 
        {
            var json = JsonConvert.SerializeObject(_allFiles.ToList(), Formatting.Indented);
            File.WriteAllText(_storagePath, json);
        }

        public void FilterFiles(string searchText)
        {
            IEnumerable<FileData> filtered = string.IsNullOrEmpty(searchText)
                ? _allFiles
                : _allFiles.Where(f =>
                    f.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            var sorted = filtered
                .OrderByDescending(f => f.IsPinned)
                .ThenByDescending(f => f.LastAccess)
                .ToList();

            _filteredFiles.Clear();
            foreach (var item in sorted)
            {
                _filteredFiles.Add(item);
            }
        }
    }
}
