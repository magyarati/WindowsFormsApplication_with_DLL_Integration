using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    class Logger
    {
        private readonly StringBuilder buffer = new StringBuilder();
        private readonly object lockObj = new object();
        private readonly TextBox textBoxOutput;

        public Logger(TextBox textBox)
        {
            textBoxOutput = textBox;
        }

        public void AppendLine(string text)
        {
            string timestamp = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff] ");
            string fullLine = timestamp + text + Environment.NewLine;

            lock (lockObj)
            {
                buffer.Append(fullLine);
            }

            if (textBoxOutput.InvokeRequired)
            {
                textBoxOutput.Invoke(new Action(() => textBoxOutput.AppendText(fullLine)));
            }
            else
            {
                textBoxOutput.AppendText(fullLine);
            }
        }

        public string GetContent()
        {
            lock (lockObj)
            {
                return buffer.ToString();
            }
        }
    }
}
