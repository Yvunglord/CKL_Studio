using CKL_Studio.Common.Interfaces;
using CKL_Studio.Presentation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Infrastructure.Services
{
    public class NamingService : INamingService
    {
        public string GeneratePath(string baseName, string directory)
        {
            Directory.CreateDirectory(directory);
            baseName = SanitizeName(baseName);
            int i = 1;
            string fileName;
            do
            {
                fileName = $"{baseName}{i}.ckl";
                i++;
            } while (File.Exists(Path.Combine(directory, fileName)));

            return Path.Combine(directory, fileName);
        }

        public string SanitizeName(string name)
            => string.Concat(name.Split(Path.GetInvalidFileNameChars()));

        public string UpdatePath(string current, string name, bool isManual)
        {
            name = SanitizeName(name);

            if (isManual)
            {
                var directory = Path.GetDirectoryName(current);
                if (directory != null)
                    return Path.Combine(directory, $"{name}.ckl");
            }
            return Path.Combine(Constants.DEFAULT_CKL_FILE_PATH, $"{name}.ckl");
        }
    }
}
