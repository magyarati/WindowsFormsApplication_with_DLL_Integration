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
        private readonly GetID.GetID getID;
        private readonly BatchingLogger logger;
        private readonly SaveHandler saveHandler;
        private readonly List<GetID.GetID> multiGetIDInstances = new List<GetID.GetID>();

        public FormMain()
        {
            InitializeComponent();

            logger = new BatchingLogger(textBoxOutput, flushIntervalMs: 100);
            saveHandler = new SaveHandler();

            getID = new GetID.GetID();
            getID.ValueChanged += getID_ValueChanged;
            getID.ErrorChanged += getID_ErrorChanged;

            toolTip.SetToolTip(buttonGo, "Elindítja a GO eljárást, ha az még nem fut.");
            toolTip.SetToolTip(buttonGoAnyway, "Elindítja a GO eljárást mindenképpen.");
            toolTip.SetToolTip(buttonStop, "Leállítja a GO eljárást, ha éppen fut.");
            toolTip.SetToolTip(buttonSave, "Menti fájlba a TextBox tartalmát.");
            toolTip.SetToolTip(buttonMultiGo, "Elindítja a GO eljárást több példányban a beállított darabszámmal");
            toolTip.SetToolTip(buttonMultiStop, "Leállítja a futó multi-példányokat");
            toolTip.SetToolTip(multiGetIDs, "GO eljárás a beállított darabszámmal");

            UpdateRunningState();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!getID.Running)
                {
                    getID.Go();
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
                if (getID.Running)
                {
                    getID.Stop();
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

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            await saveHandler.SaveLogAsync(
                getContent: () => logger.GetContent(),
                log: msg => logger.AppendLine(msg)
            );
        }

        private void getID_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                string value = getID.Value ?? "";
                string result = IsValid(value)
                    ? value + " – MEGFELELŐ"
                    : value + " – NEM MEGFELELŐ";
                logger.AppendLine(result);
            }
            catch (Exception ex)
            {
                logger.AppendLine("[HIBA] ValueChanged: " + ex.Message);
            }
        }

        private void getID_ErrorChanged(object sender, EventArgs e)
        {
            string error = getID.ErrorMessage ?? "";
            logger.AppendLine("[HIBA]: " + error);
        }

        private void multiGetID_ValueChanged(object sender, EventArgs e)
        {
            var inst = sender as GetID.GetID;
            int idx = multiGetIDInstances.IndexOf(inst) + 1;
            string value = inst.Value ?? "";
            string result = IsValid(value)
                ? $"{value} – MEGFELELŐ (#{idx})"
                : $"{value} – NEM MEGFELELŐ (#{idx})";
            logger.AppendLine(result);
        }

        private void multiGetID_ErrorChanged(object sender, EventArgs e)
        {
            var inst = sender as GetID.GetID;
            int idx = multiGetIDInstances.IndexOf(inst) + 1;
            string error = inst.ErrorMessage ?? "";
            logger.AppendLine($"[HIBA] (#{idx}): {error}");
        }

        private bool IsValid(string value)
        {
            return Regex.IsMatch(value, @"^[A-Z]\d{4}$");
        }

        private void UpdateRunningState()
        {
            bool singleRunning = getID.Running;
            string singleText = singleRunning
                ? "Állapot: Futtatás alatt"
                : "Állapot: Leállítva";

            int multiRunning = multiGetIDInstances.Count(inst => inst.Running);
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

        private void buttonMultiGo_Click(object sender, EventArgs e)
        {
            foreach (var inst in multiGetIDInstances)
                if (inst.Running) inst.Stop();
            multiGetIDInstances.Clear();

            int darab = (int)multiGetIDs.Value;
            if (darab < 1)
            {
                logger.AppendLine(">> MultiGo: érvénytelen darabszám.");
                return;
            }

            for (int i = 1; i <= darab; i++)
            {
                var inst = new GetID.GetID();

                inst.ValueChanged += multiGetID_ValueChanged;
                inst.ErrorChanged += multiGetID_ErrorChanged;

                inst.Go();
                multiGetIDInstances.Add(inst);

                logger.AppendLine($">> Multi GO eljárás indítva: példány #{i}");
            }

            logger.AppendLine($">> Összesen {multiGetIDInstances.Count} példány fut.");
            UpdateRunningState();
        }

        private void buttonMultiStop_Click(object sender, EventArgs e)
        {
            if (multiGetIDInstances.Count == 0)
            {
                logger.AppendLine(">> Nincsenek futó Multi-példányok.");
                return;
            }

            int stopped = 0;
            foreach (var inst in multiGetIDInstances)
            {
                if (inst.Running)
                {
                    inst.Stop();
                    stopped++;
                }
            }

            logger.AppendLine($">> Leállítva {stopped} multi-példány.");
            multiGetIDInstances.Clear();
            UpdateRunningState();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            logger.Dispose();
        }

        private void buttonGoAnyway_Click(object sender, EventArgs e)
        {
            try
            {
                getID.Go();
                logger.AppendLine(">> GO eljárás elindítva");
                UpdateRunningState();
            }
            catch (Exception ex)
            {
                logger.AppendLine("[HIBA] Start: " + ex.Message);
            }
        }
    }
}
