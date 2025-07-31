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
        private readonly Logger logger;
        private readonly SaveHandler saveHandler;

        public FormMain()
        {
            InitializeComponent();

            logger = new Logger(textBoxOutput);
            saveHandler = new SaveHandler();

            getID = new GetID.GetID();
            getID.ValueChanged += getID_ValueChanged;
            getID.ErrorChanged += getID_ErrorChanged;

            toolTip.SetToolTip(buttonGo, "Elindítja a GO eljárást, ha az még nem fut.");
            toolTip.SetToolTip(buttonStop, "Leállítja a GO eljárást, ha éppen fut.");
            toolTip.SetToolTip(buttonSave, "Menti fájlba a TextBox tartalmát.");

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

        private bool IsValid(string value)
        {
            return Regex.IsMatch(value, @"^[A-Z]\d{4}$");
        }

        private void UpdateRunningState()
        {
            bool isRunning = getID.Running;
            string status = isRunning ? "Állapot: Futtatás alatt" : "Állapot: Leállítva";

            if (labelStatus.InvokeRequired)
            {
                labelStatus.Invoke(new Action(() => labelStatus.Text = status));
            }
            else
            {
                labelStatus.Text = status;
            }
        }
    }
}
