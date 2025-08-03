using System;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    public partial class FormMain : Form, IMainView
    {
        public FormMain( ILogger logger)
        {
            this.logger = logger;
        }

        private ILogger logger;

        public event EventHandler GoRequested;
        public event EventHandler StopRequested;
        public event EventHandler SaveRequested;
        public event EventHandler ViewClosed;
        public event EventHandler RefreshStatus;

        public bool IgnoreRunningState => checkBoxIgnoreRunningState.Checked;

        public void Initialize()
        {
            InitializeComponent();

            toolTip.SetToolTip(buttonGo, "Elindítja a GO eljárást, ha az még nem fut.");
            toolTip.SetToolTip(buttonStop, "Leállítja a GO eljárást, ha éppen fut.");
            toolTip.SetToolTip(buttonSave, "Menti fájlba a TextBox tartalmát.");
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            logger.InitializeLogger(textBoxOutput,
            checkBoxShowTimestamps,
            checkBoxShowLineNumbers,
            flushIntervalMs: 100,
            maxChars: 32 * 1024 * 1024);
        }

        private void SetLabelTextSafe(Label lbl, string text)
        {
            if (lbl.InvokeRequired)
                lbl.Invoke(new Action(() => lbl.Text = text));
            else
                lbl.Text = text;
        }

        private void ButtonGo_Click(object sender, EventArgs e)
        {
            GoRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            StopRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveRequested?.Invoke(this, EventArgs.Empty);
        }

        public void DisplayLog(string text) => logger.AppendLine(text);
        public void UpdateSingleStatus(string s) => SetLabelTextSafe(labelStatus, s);
        public void UpdateMemoryStatus(string s) => SetLabelTextSafe(labelMemory, s);
        public void UpdateTextboxMemoryStatus(string s) => SetLabelTextSafe(labelTextboxMemory, s);

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            ViewClosed.Invoke(this, EventArgs.Empty);
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            RefreshStatus.Invoke(this, EventArgs.Empty);
        }

        private void NumericUpDownTextBoxLength_ValueChanged(object sender, EventArgs e)
        {
            int newMaxChars = (int)numericUpDownTextBoxLength.Value * 1024 * 1024;
            logger.SetMaxChars(newMaxChars);
            textBoxOutput.MaxLength = newMaxChars;
        }

        public string GetContent()
        {
            return textBoxOutput.Text;
        }
    }
}
