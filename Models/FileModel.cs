namespace CKL_Studio.MVVM.Model
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime LastChange { get; set; }

        public FileModel(string fileName, string filePath, DateTime lastChange)
        {
            FileName = fileName;
            FilePath = filePath;
            LastChange = lastChange;
        }
        public override string ToString()
        {
            return FileName;
        }
    }
}
