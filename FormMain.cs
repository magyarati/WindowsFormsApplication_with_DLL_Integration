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
        private readonly object logLock = new object();
        private readonly Logger logger;

        public FormMain()
        {
            InitializeComponent();

            logger = new Logger(textBoxOutput);

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
            while (true)
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Text fájl (*.txt)|*.txt";
                    sfd.Title = "Mentés fájlba";
                    sfd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    sfd.FileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string content;
                            lock (logLock)
                            {
                                content = logger.GetContent();
                            }

                            await Task.Run(() =>
                            {
                                using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                                using (StreamWriter writer = new StreamWriter(fs))
                                {
                                    writer.Write(content);
                                }
                            });

                            logger.AppendLine($">> Log sikeresen elmentve: {sfd.FileName}");
                            break;
                        }
                        catch (IOException ioex)
                        {
                            logger.AppendLine($"[HIBA] Mentés sikertelen, fájl használatban lehet: {ioex.Message}");
                            DialogResult retry = MessageBox.Show(
                                $"Nem sikerült menteni, a fájl lehet, hogy használatban van:\n\n{ioex.Message}\n\nPróbálsz másik fájlt?",
                                "Hiba mentéskor",
                                MessageBoxButtons.RetryCancel,
                                MessageBoxIcon.Warning);

                            if (retry == DialogResult.Cancel)
                            {
                                logger.AppendLine(">> Felhasználó megszakította a mentést.");
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.AppendLine($"[HIBA] Ismeretlen mentési hiba: {ex.Message}");
                            break;
                        }
                    }
                    else
                    {
                        logger.AppendLine(">> Mentés megszakítva, nem lett fájl kiválasztva.");
                        break;
                    }
                }
            }
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
