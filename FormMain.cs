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
    public partial class FormMain : Form, IMainView
    {
        private MainPresenter _presenter;

        public event EventHandler GoRequested;
        public event EventHandler StopRequested;
        public event EventHandler SaveRequested;
        public event EventHandler MultiGoRequested;
        public event EventHandler MultiStopRequested;

        public int MultiGetIDsValue => (int)multiGetIDs.Value;
        public bool IgnoreRunningState => checkBoxIgnoreRunningState.Checked;

        public void DisplayLog(string text) => logger.AppendLine(text);
        public void UpdateSingleStatus(string s) => SetLabelTextSafe(labelStatus, s);
        public void UpdateMultiStatus(string s) => SetLabelTextSafe(labelMultiStatus, s);
        
        private readonly ISaveHandler saveHandler;
        private readonly BatchingLogger logger;

        public FormMain()
        {
            InitializeComponent();

            saveHandler = new SaveHandler();
            logger = new BatchingLogger(
            textBoxOutput,
            saveHandler,
            checkBoxShowTimestamps,
            checkBoxShowLineNumbers,
            flushIntervalMs: 100,
            maxChars: 20_000_000);

            _presenter = new MainPresenter(this, saveHandler, logger);

            toolTip.SetToolTip(buttonGo, "Elindítja a GO eljárást, ha az még nem fut.");
            toolTip.SetToolTip(buttonStop, "Leállítja a GO eljárást, ha éppen fut.");
            toolTip.SetToolTip(buttonSave, "Menti fájlba a TextBox tartalmát.");
            toolTip.SetToolTip(buttonMultiGo, "Elindítja a GO eljárást több példányban a beállított darabszámmal");
            toolTip.SetToolTip(buttonMultiStop, "Leállítja a futó multi-példányokat");
            toolTip.SetToolTip(multiGetIDs, "GO eljárás a beállított darabszámmal");
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
            GoRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonMultiGo_Click(object sender, EventArgs e)
        {
            MultiGoRequested?.Invoke(this, EventArgs.Empty);

        }

        private void buttonMultiStop_Click(object sender, EventArgs e)
        {
            MultiStopRequested?.Invoke(this, EventArgs.Empty);

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _presenter.OnClosing();
        }
    }
}
