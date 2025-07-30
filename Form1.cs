using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    public partial class FormMain : Form
    {
        private GetID.GetID getID;
        public FormMain()
        {
            InitializeComponent();

            getID = new GetID.GetID();
            getID.ValueChanged += getID_ValueChanged;
            getID.ErrorChanged += getID_ErrorChanged;
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            try
            {
                getID.Go();
                AppendLine(">> GO eljárás elindítva");
            }
            catch (Exception ex)
            {
                AppendLine("[HIBA] Start: " + ex.Message);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                getID.Stop();
                AppendLine(">> STOP eljárás elindítva");
            }
            catch (Exception ex)
            {
                AppendLine("[HIBA] Stop: " + ex.Message);
            }
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            string content = textBoxOutput.Text;

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
                            string filePath = sfd.FileName;

                            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                await writer.WriteAsync(content);
                            }

                            MessageBox.Show("Sikeres mentés.", "Mentés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                        catch (IOException ioex)
                        {
                            DialogResult retry = MessageBox.Show(
                                $"Nem sikerült menteni, a fájl lehet, hogy használatban van:\n\n{ioex.Message}\n\nPróbálsz másik fájlt?",
                                "Hiba mentéskor",
                                MessageBoxButtons.RetryCancel,
                                MessageBoxIcon.Warning);

                            if (retry == DialogResult.Cancel)
                                break;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ismeretlen hiba történt: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void getID_ValueChanged(object sender, EventArgs e)
        {
            string value = getID.Value ?? "";
            string result = IsValid(value)
                ? value + " – MEGFELELŐ"
                : value + " – NEM MEGFELELŐ";
            AppendLine(result);
        }

        private void getID_ErrorChanged(object sender, EventArgs e)
        {
            string error = getID.ErrorMessage ?? "";
            AppendLine("[HIBA]: " + error);
        }

        private void AppendLine(string text)
        {
            if (textBoxOutput.InvokeRequired)
            {
                textBoxOutput.Invoke(new Action<string>(AppendLine), text);
            }
            else
            {
                string timestamp = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff] ");
                textBoxOutput.AppendText(timestamp + text + Environment.NewLine);
            }
        }

        private bool IsValid(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[A-Z]\d{4}$");
        }
    }
}
