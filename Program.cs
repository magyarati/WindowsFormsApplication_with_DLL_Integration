using System;
using System.IO;
using System.Threading;
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

            // Globális kivételkezelők
            Application.ThreadException += (sender, args) =>
            {
                MessageBox.Show(
                    $"Nem kezelt hiba: {args.Exception.Message}\n\n{args.Exception.StackTrace}",
                    "Kritikus hiba",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            };
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = (Exception)args.ExceptionObject;
                MessageBox.Show(
                    $"Végzetes hiba: {ex.Message}\n\n{ex.StackTrace}",
                    "Végzetes hiba",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            };
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                MessageBox.Show(
                    $"Nem várt feladatkivétel: {e.Exception.Message}\n\n{e.Exception.StackTrace}",
                    "Feladatkivétel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            };

            var saveHandler = new SaveHandler();
            var batchingLogger = new BatchingLogger(saveHandler);
            var mainForm = new FormMain(batchingLogger);
            var mainPresenter = new MainPresenter(mainForm, saveHandler, batchingLogger);

            var view = mainPresenter.Initialize();
            view.Initialize();

            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            batchingLogger.InitializeLogger(
                view: view,
                silentFileBaseName: "log",
                silentFileDirectory: logDirectory,
                flushIntervalMs: 100,
                maxChars: 32 * 1024 * 1024
            );

            // Alkalmazás futtatása
            try
            {
                Application.Run(view);
            }
            finally
            {
                batchingLogger.Dispose();
            }
        }
    }
}
