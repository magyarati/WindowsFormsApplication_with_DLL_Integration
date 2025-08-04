using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    public class BatchingLogger : ILogger, IDisposable
    {
        private ISaveHandler saveHandler;
        private ILogView view;
        private System.Windows.Forms.Timer flushTimer;
        private int maxChars;
        private int currentChars;
        private string silentFileBaseName;
        private string silentFileDirectory;
        private int segmentCounter;
        private int lineCounter;
        private readonly ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

        public BatchingLogger(ISaveHandler saveHandler)
        {
            this.saveHandler = saveHandler;
        }
        
        public void InitializeLogger(
            ILogView view,
            string silentFileBaseName = "log",
            string silentFileDirectory = ".\\logs",
            int flushIntervalMs = 100,
            int maxChars = 100_000)
        {
            this.view = view;
            this.maxChars = maxChars;
            this.silentFileBaseName = silentFileBaseName;
            this.silentFileDirectory = silentFileDirectory;

            Directory.CreateDirectory(silentFileDirectory);

            flushTimer = new System.Windows.Forms.Timer { Interval = flushIntervalMs };
            flushTimer.Tick += OnFlushTimerTick;
            flushTimer.Start();
        }

        private void OnFlushTimerTick(object sender, EventArgs e) => Flush();

        public void AppendLine(string text)
        {
            var prefix = new StringBuilder();

            if (view.ShowLineNumbers)
                prefix.AppendFormat("{0,8}: ", lineCounter++);

            if (view.ShowTimestamps)
                prefix.Append(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff] "));

            prefix.Append(text)
                  .AppendLine();

            queue.Enqueue(prefix.ToString());
        }

        private void Flush()
        {
            if (queue.IsEmpty)
                return;

            var sb = new StringBuilder();
            while (queue.TryDequeue(out var line))
                sb.Append(line);

            AppendToViewWithTrimming(sb.ToString());
        }

        private void AppendToViewWithTrimming(string batch)
        {
            view.AppendText(batch);
            currentChars += batch.Length;

            if (currentChars <= maxChars)
                return;

            int threshold = maxChars / 2;
            string existing = view.GetText();
            int cutIndex = existing.IndexOf('\n', threshold);
            if (cutIndex < 0)
                cutIndex = threshold;
            else
                cutIndex += 1;

            string removed = existing.Substring(0, cutIndex);
            string kept = existing.Substring(cutIndex);

            view.SetText(kept);
            currentChars = kept.Length;

            if (view.SaveSilentSegments)
            {
                string path = GetNextSilentFilePath();
                _ = saveHandler.SaveLogAsync(
                    removed,
                    log: AppendLine,
                    silent: true,
                    silentFilePath: path
                );
            }
        }

        private string GetNextSilentFilePath()
        {
            int index = Interlocked.Increment(ref segmentCounter);
            string fileName = $"{silentFileBaseName}_{index:000}.log";
            return Path.Combine(silentFileDirectory, fileName);
        }

        public int GetCurrentTextSize() => view.GetText().Length;

        public void SetMaxChars(int newMax) => maxChars = newMax;

        public void Dispose()
        {
            flushTimer.Stop();
            flushTimer.Tick -= OnFlushTimerTick;
            flushTimer.Dispose();
        }
    }
}
