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
        event EventHandler MultiGoRequested;
        event EventHandler MultiStopRequested;

        int MultiGetIDsValue { get; }
        bool IgnoreRunningState { get; }

        void DisplayLog(string text);
        void UpdateSingleStatus(string status);
        void UpdateMultiStatus(string status);
    }
}
