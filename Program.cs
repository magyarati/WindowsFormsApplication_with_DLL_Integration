using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.ThreadException += (sender, args) =>
            {
                MessageBox.Show("Nem kezelt hiba: " + args.Exception.Message, "Kritikus hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                MessageBox.Show("Végzetes hiba: " + ((Exception)args.ExceptionObject).Message, "Végzetes hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            Application.Run(new FormMain());
        }
    }
}
