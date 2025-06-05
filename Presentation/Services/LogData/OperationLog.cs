using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Presentation.Services.LogData
{
    public class OperationLog
    {
        public DateTime Timestamp { get; set; }
        public string OperationName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Timestamp:HH:mm:ss} | {OperationName} | {FileName} | {AdditionalInfo}";
        }
    }
}
