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

        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            AppendLine("[Go eljárás elinditása...]");
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            AppendLine("[Stop eljárás elinditása...]");
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text fájl (*.txt)|*.txt";
                sfd.Title = "Mentés fájlba";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(sfd.FileName, textBoxOutput.Text);
                        MessageBox.Show("Sikeres mentés.", "Mentés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Mentési hiba: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void AppendLine(string text)
        {
            string timestamp = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff] ");
            textBoxOutput.AppendText(timestamp + text + Environment.NewLine);
        }
    }
}

