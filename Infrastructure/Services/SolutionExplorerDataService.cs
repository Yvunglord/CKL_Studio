using CKL_Studio.Common.Interfaces;
using CKL_Studio.Infrastructure.Static;
using CKLDrawing;
using CKLLib;
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
    public class SolutionExplorerDataService : IDataService<CKL>
    {
        private readonly CKL _originalCkl;
        private readonly ObservableCollection<CKL> _solutionItems = new ObservableCollection<CKL>();
        private readonly string _originalDirectory;
        public ObservableCollection<CKL> Items => _solutionItems;

        public SolutionExplorerDataService(CKL originalCkl)
        {
            _originalCkl = originalCkl;
            _originalDirectory = GetRootDirectoryName();
        }

        private string GetRootDirectoryName()
        {
            var path = Path.GetDirectoryName(_originalCkl.FilePath);
            if (path != null)
                return path;

            return string.Empty;
        }   

        public void Add(CKL item)
        {
            if (!_solutionItems.Any(f => f.FilePath == item.FilePath))
            {
                if (_originalDirectory.Equals(Path.GetDirectoryName(item.FilePath), StringComparison.Ordinal))
                { 
                    _solutionItems.Add(item);
                }    
            }
        }

        public void AddUnsafe(CKL item)
        {
            if (!_solutionItems.Any(f => f.FilePath == item.FilePath))
            {
                _solutionItems.Add(item);
            }
        }

        public void Delete(CKL item)
        {
            _solutionItems?.Remove(item);
        }

        public void Clear()
        {
            _solutionItems.Clear();
        }

        public CKL? Get(string identifier)
        {
            var ckl = _solutionItems.FirstOrDefault(c => Path.GetFileName(c.FilePath) == identifier);
            if (ckl != null)
                return ckl;

            return null;
        }

        public IEnumerable<CKL> GetAll() => _solutionItems;

        public void Update(CKL item)
        {
            var existing = Get(item.FilePath);
            if (existing != null)
            {
                existing.Source = item.Source;
                existing.GlobalInterval = item.GlobalInterval;
                existing.Relation = item.Relation;
            }
        }

        public void Load(CKLView main)
        { 
            _solutionItems.Clear();

            Add(main.Ckl);
            var root = Path.GetDirectoryName(main.Ckl.FilePath);

            if (root != null)
            {
                foreach (var path in Directory.GetFiles(root, "*.ckl"))
                {
                    if (path != main.Ckl.FilePath)
                    { 
                        var related = CKL.GetFromFile(path);
                        related.FilePath = path;
                        if (related != null && BinaryCKLOperationsValidator.CanPerformOperation(main.Ckl, related))
                            Add(related);
                    }
                }
            }

        }
    }
}
