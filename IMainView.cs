using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication_with_DLL_Integration
{
    interface IMainView
    {
        event EventHandler GoRequested;
        event EventHandler StopRequested;
        event EventHandler SaveRequested;

        bool IgnoreRunningState { get; }

        void DisplayLog(string text);
        void UpdateSingleStatus(string status);
        void UpdateMemoryStatus(string formatted);
        void UpdateTextboxMemoryStatus(string formatted);
    }
}
