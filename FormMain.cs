using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    public partial class FormMain : Form
    {
        private GetIDWorker _singleWorker;
        private readonly List<GetIDWorker> _multiWorkers = new List<GetIDWorker>();

        private readonly BatchingLogger logger;
        private readonly SaveHandler saveHandler;

        public FormMain()
        {
            InitializeComponent();

            logger = new BatchingLogger(textBoxOutput, flushIntervalMs: 100);
            saveHandler = new SaveHandler();

            _singleWorker = new GetIDWorker();
            _singleWorker.ValueReceived += (s, v) => LoggerLog(v, 0);
            _singleWorker.ErrorReceived += (s, e) => LoggerError(e, 0);

            toolTip.SetToolTip(buttonGo, "Elindítja a GO eljárást, ha az még nem fut.");
            toolTip.SetToolTip(buttonGoAnyway, "Elindítja a GO eljárást mindenképpen.");
            toolTip.SetToolTip(buttonStop, "Leállítja a GO eljárást, ha éppen fut.");
            toolTip.SetToolTip(buttonSave, "Menti fájlba a TextBox tartalmát.");
            toolTip.SetToolTip(buttonMultiGo, "Elindítja a GO eljárást több példányban a beállított darabszámmal");
            toolTip.SetToolTip(buttonMultiStop, "Leállítja a futó multi-példányokat");
            toolTip.SetToolTip(multiGetIDs, "GO eljárás a beállított darabszámmal");

            UpdateRunningState();
        }

        private bool IsValid(string value) =>
            Regex.IsMatch(value, @"^[A-Z]\d{4}$");

        private void LoggerLog(string value, int idx)
        {
            string suffix = idx == 0 ? "" : $" (#{idx})";
            string text = IsValid(value)
                ? $"{value} – MEGFELELŐ{suffix}"
                : $"{value} – NEM MEGFELELŐ{suffix}";
            logger.AppendLine(text);
        }

        private void LoggerError(string error, int idx)
        {
            string suffix = idx == 0 ? "" : $" (#{idx})";
            logger.AppendLine($"[HIBA]{suffix}: {error}");
        }

        private void UpdateRunningState()
        {
            string singleText = _singleWorker.Running
                ? "Állapot: Futtatás alatt"
                : "Állapot: Leállítva";

            int multiRunning = _multiWorkers.Count(w => w.Running);
            string multiText = $"Állapot: {multiRunning} szál fut";

            SetLabelTextSafe(labelStatus, singleText);
            SetLabelTextSafe(labelMultiStatus, multiText);
        }

        private void SetLabelTextSafe(Label lbl, string text)
        {
            if (lbl.InvokeRequired)
                lbl.Invoke(new Action(() => lbl.Text = text));
            else
                lbl.Text = text;
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_singleWorker.Running)
                {
                    _singleWorker.Start();
                    logger.AppendLine(">> GO eljárás elindítva");
                    UpdateRunningState();
                }
                else
                {
                    logger.AppendLine(">> Már fut a GO eljárás, újraindítás nem történt.");
                }
            }
            catch (Exception ex)
            {
                logger.AppendLine("[HIBA] Start: " + ex.Message);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (_singleWorker.Running)
                {
                    _singleWorker.Stop();
                    logger.AppendLine(">> STOP eljárás elindítva");
                    UpdateRunningState();
                }
                else
                {
                    logger.AppendLine(">> A GO eljárás nem fut.");
                }
            }
            catch (Exception ex)
            {
                logger.AppendLine("[HIBA] Stop: " + ex.Message);
            }
        }

        private void buttonGoAnyway_Click(object sender, EventArgs e)
        {
            try
            {
                _singleWorker.Start();
                logger.AppendLine(">> GO eljárás mindenképpen elindítva");
                UpdateRunningState();
            }
            catch (Exception ex)
            {
                logger.AppendLine("[HIBA] Start: " + ex.Message);
            }
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                await saveHandler.SaveLogAsync(
                    getContent: () => logger.GetContent(),
                    log: msg => logger.AppendLine(msg));
            }
            catch (Exception ex)
            {
                logger.AppendLine("[HIBA] Mentés: " + ex.Message);
            }
        }

        private void buttonMultiGo_Click(object sender, EventArgs e)
        {
            try
            {
                Parallel.ForEach(_multiWorkers, w => w.Dispose());
                _multiWorkers.Clear();

                int darab = (int)multiGetIDs.Value;
                if (darab < 1)
                {
                    logger.AppendLine(">> MultiGo: érvénytelen darabszám.");
                    return;
                }

                for (int i = 0; i < darab; i++)
                {
                    var worker = new GetIDWorker();
                    int idx = i + 1;
                    worker.ValueReceived += (s, v) => LoggerLog(v, idx);
                    worker.ErrorReceived += (s, err) => LoggerError(err, idx);
                    _multiWorkers.Add(worker);
                }

                Parallel.For(0, _multiWorkers.Count, i =>
                {
                    _multiWorkers[i].Start();
                    logger.AppendLine($">> Multi GO eljárás indítva: példány #{i + 1}");
                });

                logger.AppendLine($">> Összesen {_multiWorkers.Count} példány fut.");
                UpdateRunningState();
            }
            catch (Exception ex)
            {
                logger.AppendLine("[HIBA] MultiGo: " + ex.Message);
            }
        }

        private void buttonMultiStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (_multiWorkers.Count == 0)
                {
                    logger.AppendLine(">> Nincsenek futó Multi-példányok.");
                    return;
                }

                Parallel.ForEach(_multiWorkers, w => w.Dispose());
                logger.AppendLine($">> Leállítva {_multiWorkers.Count} multi-példány.");
                _multiWorkers.Clear();
                UpdateRunningState();
            }
            catch (Exception ex)
            {
                logger.AppendLine("[HIBA] MultiStop: " + ex.Message);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            _singleWorker.Dispose();
            foreach (var w in _multiWorkers)
                w.Dispose();

            logger.Dispose();
        }
    }
}
