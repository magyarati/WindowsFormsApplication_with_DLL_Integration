using System;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    public interface ILogger : IDisposable
    {
        void AppendLine(string text);
        void SetMaxChars(int newMax);
        void InitializeLogger(
            ILogView view,
            string silentFileBaseName = "log",
            string silentFileDirectory = ".\\logs",
            int flushIntervalMs = 100,
            int maxChars = 100_000);
        int GetCurrentTextSize();
    }

}
