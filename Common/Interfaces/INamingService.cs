using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Common.Interfaces
{
    public interface INamingService
    {
        string GeneratePath(string name, string directory);
        string UpdatePath(string current, string name, bool isManual);
        string SanitizeName(string name);
    }
}
