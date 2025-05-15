using CKL_Studio.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Infrastructure.Services
{
    public class SearchHistoryDataService : IDataService<string>
    {
        private const string _storagePath = "search_history.json";
        private readonly ObservableCollection<string> _searchHistory = new ObservableCollection<string>();

        public ObservableCollection<string> Items => _searchHistory;

        public void Add(string item)
        {
            if (!string.IsNullOrWhiteSpace(item) && !_searchHistory.Contains(item))
            {
                _searchHistory.Insert(0, item);
                if (_searchHistory.Count > 5) _searchHistory.RemoveAt(5);
            }
        }

        public void Delete(string item)
        {
            _searchHistory.Remove(item);
        }

        public string Get(string identifier)
        {
            var item = _searchHistory.FirstOrDefault(s => s.Equals(identifier));
            if (item != null)
                return item;

            throw new NotImplementedException($"Text with identifier '{identifier}' not found.");
        }

        public IEnumerable<string> GetAll()
        {
            return _searchHistory;
        }

        public void Update(string item)
        {
            Add(item);
        }

        public void Load()
        {
            if (File.Exists(_storagePath))
            { 
                var json = File.ReadAllText(_storagePath);
                var history = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();

                _searchHistory.Clear();
                foreach (var item in history)
                { 
                    _searchHistory.Add(item);
                }
            }
        }

        public void Save()
        { 
            var json = JsonConvert.SerializeObject(_searchHistory.ToList(), Formatting.Indented);
            File.WriteAllText(_storagePath, json);  
        }
    }
}
