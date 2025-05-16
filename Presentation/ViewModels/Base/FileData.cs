using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Presentation.ViewModels.Base
{
    [JsonObject]
    public class FileData : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _path = string.Empty;
        private DateTime _lastAccess;
        private FileInfo _file = null!;
        private bool _isPinned;

        public FileData(FileInfo file) 
        {
            _file = file;
            _name = file.Name;
            _path = file.FullName;
            _lastAccess = file.LastAccessTime;
        }

        //Конструктор для создания JSON.NET объекта при десереализации
        public FileData() { }

        [JsonProperty]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonProperty]
        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged();
                }
            }
        }

        [JsonProperty]
        public DateTime LastAccess
        {
            get => _lastAccess;
            set
            {
                if (_lastAccess != value)
                {
                    _lastAccess = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LastAccessFormatted));
                }
            }
        }

        [JsonIgnore]
        public string LastAccessFormatted => _lastAccess.ToString("dd.MM.yyyy HH:mm");

        [JsonIgnore]
        public FileInfo File => _file;

        public bool IsPinned
        {
            get => _isPinned;
            set
            {
                if (_isPinned != value)
                {
                    _isPinned = value;
                    OnPropertyChanged(nameof(IsPinned));
                }
            }
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            if (!string.IsNullOrEmpty(_path))
            { 
                _file = new FileInfo(_path);
                if (_file.Exists)
                { 
                    _name = _file.Name;
                    _lastAccess = _file.LastAccessTime;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
