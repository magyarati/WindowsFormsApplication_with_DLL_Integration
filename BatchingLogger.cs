using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    public class BatchingLogger : IDisposable
    {
        private readonly ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        private readonly TextBox textBox;
        private readonly System.Windows.Forms.Timer flushTimer;
        private readonly ISaveHandler saveHandler;
        private readonly int maxChars;
        private int currentChars = 0;

        private readonly string silentFileBaseName;
        private readonly string silentFileDirectory;
        private int segmentCounter = 0;

        private readonly CheckBox checkBoxShowTimestamps;
        private readonly CheckBox checkBoxShowLineNumbers;
        private int lineCounter = 1;

        public BatchingLogger(
            TextBox textBox,
            ISaveHandler saveHandler,
            CheckBox checkBoxShowTimestamps,
            CheckBox checkBoxShowLineNumbers,
            string silentFileBaseName = "log",
            string silentFileDirectory = ".\\logs",
            int flushIntervalMs = 100,
            int maxChars = 100_000)
        {
            this.textBox = textBox;
            this.saveHandler = saveHandler;
            this.checkBoxShowTimestamps = checkBoxShowTimestamps;
            this.checkBoxShowLineNumbers = checkBoxShowLineNumbers;
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
            string linePrefix = "";
            int currentLine = lineCounter++;

            if (checkBoxShowLineNumbers?.Checked == true)
            {
                linePrefix += $"{currentLine,8}: ";
            }

            if (checkBoxShowTimestamps?.Checked == true)
            {
                linePrefix += DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff] ");
            }

            string line = linePrefix + text + Environment.NewLine;
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
            AppendToTextBoxWithTrimming(batch);
        }

        private void AppendToTextBoxWithTrimming(string batch)
        {
            textBox.AppendText(batch);
            currentChars += batch.Length;

            if (currentChars <= maxChars)
                return;

            int threshold = maxChars / 2;
            string existingText = textBox.Text;
            int cutIndex = existingText.IndexOf('\n', threshold);
            if (cutIndex < 0)
                cutIndex = threshold;
            else
                cutIndex += 1;

            string removedText = existingText.Substring(0, cutIndex);
            textBox.Text = existingText.Substring(cutIndex);
            currentChars = textBox.Text.Length;

            string path = GetNextSilentFilePath();
            _ = saveHandler.SaveLogAsync(
                getContent: () => removedText,
                log: _ => { },
                silent: true,
                silentFilePath: path
            );
        }

        private string GetNextSilentFilePath()
        {
            int index = Interlocked.Increment(ref segmentCounter);
            string fileName = $"{silentFileBaseName}_{index:000}.log";
            return Path.Combine(silentFileDirectory, fileName);
        }

        public string GetContent()
        {
           return textBox.Text.ToString();
        }

        public int GetCurrentTextSize() => textBox.Text.Length;

        public void Dispose()
        {
            flushTimer.Stop();
            flushTimer.Tick -= OnFlushTimerTick;
            flushTimer.Dispose();
        }
    }
}
