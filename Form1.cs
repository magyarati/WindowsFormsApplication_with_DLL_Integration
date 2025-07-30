using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            AppendLine("[TextBox tartalmának Mentése...]");
        }

        private void AppendLine(string text)
        {
            textBoxOutput.AppendText(text + Environment.NewLine);
        }
    }
}

