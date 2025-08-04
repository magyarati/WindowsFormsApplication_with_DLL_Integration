using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace WindowsFormsApplication_with_DLL_Integration
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += (sender, args) =>
            {
                MessageBox.Show($"Nem kezelt hiba: {args.Exception.Message}\n\n{args.Exception.StackTrace}",
                    "Kritikus hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = (Exception)args.ExceptionObject;
                MessageBox.Show($"Végzetes hiba: {ex.Message}\n\n{ex.StackTrace}",
                    "Végzetes hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateScopes = true
            });

            var mainPresenter = ServiceProvider.GetRequiredService<IMainPresenter>();
            var view = mainPresenter.Initialize();
            view.Initialize();

            var batchingLogger = ServiceProvider.GetRequiredService<BatchingLogger>();
            batchingLogger.InitializeLogger(
            view: (ILogView)view,
            silentFileBaseName: "log",
            silentFileDirectory: ".\\logs",
            flushIntervalMs: 100,
            maxChars: 32 * 1024 * 1024
                );
            
            Application.Run(view);

            (ServiceProvider as IDisposable)?.Dispose();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILogView>(sp => sp.GetRequiredService<FormMain>());
            services.AddTransient<ISaveHandler, SaveHandler>();
            services.AddSingleton<BatchingLogger>();
            services.AddSingleton<ILogger>(sp => sp.GetRequiredService<BatchingLogger>());
            services.AddTransient<IMainPresenter, MainPresenter>();
            services.AddTransient<IMainView, FormMain>();
        }
    }
}
