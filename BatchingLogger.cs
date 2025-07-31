using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    public class BatchingLogger : IDisposable
    {
        private readonly ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        private readonly StringBuilder buffer = new StringBuilder();
        private readonly TextBox textBox;
        private readonly Timer flushTimer;
        private readonly object bufferLock = new object();

        public BatchingLogger(TextBox textBox, int flushIntervalMs = 100)
        {
            this.textBox = textBox;
            flushTimer = new Timer { Interval = flushIntervalMs };
            flushTimer.Tick += (s, e) => Flush();
            flushTimer.Start();
        }

        public void AppendLine(string text)
        {
            string line = DateTime.Now
                .ToString("[yyyy-MM-dd HH:mm:ss.fff] ")
                        + text + Environment.NewLine;
            queue.Enqueue(line);
        }

        private void Flush()
        {
            if (queue.IsEmpty)
                return;

            var sb = new StringBuilder();
            while (queue.TryDequeue(out string line))
                sb.Append(line);

            string batch = sb.ToString();

            lock (bufferLock)
            {
                buffer.Append(batch);
            }

            if (textBox.InvokeRequired)
                textBox.BeginInvoke(new Action(() => textBox.AppendText(batch)));
            else
                textBox.AppendText(batch);
        }

        public string GetContent()
        {
            lock (bufferLock)
            {
                return buffer.ToString();
            }
        }

        public void Dispose()
        {
            flushTimer.Stop();
            flushTimer.Tick -= (s, e) => Flush();
            flushTimer.Dispose();
        }
    }
}
