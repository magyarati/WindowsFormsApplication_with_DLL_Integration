using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication_with_DLL_Integration
{
    public interface ISaveHandler
    {
        Task SaveLogAsync(
            Func<string> getContent,
            Action<string> log,
            bool silent = false,
            string silentFilePath = null);
    }
}
