using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication_with_DLL_Integration
{
    class MainPresenter : IMainPresenter
    {
        private IMainView _view;
        private ISaveHandler _saveHandler;
        private ILogger _logger;
        private GetIDWorker _singleWorker;
        private Timer _memoryTimer;

        public MainPresenter(IMainView view, ISaveHandler saveHandler, ILogger logger)
        {
            _view = view;
            _saveHandler = saveHandler;
            _logger = logger;
        }

        public FormMain Initialize()
        {
            _singleWorker = new GetIDWorker();
            _singleWorker.ValueReceived += (s, v) => LoggerLog(v, 0);
            _singleWorker.ErrorReceived += (s, e) => LoggerError(e, 0);

            _view.GoRequested += (sender, e) => OnGo();
            _view.StopRequested += (sender, e) => OnStop();
            _view.SaveRequested += (sender, e) => OnSave();

            _view.ViewClosed += OnViewClosed;
            _view.RefreshStatus += OnRefreshStatus;

            _memoryTimer = new Timer { Interval = 3000 };
            _memoryTimer.Tick += (s, e) =>
            {
                UpdateMemoryStatus();
                UpdateTextboxMemoryStatus();
            };
            _memoryTimer.Start();

            return _view as FormMain;
        }

        public string GetContent()
        {
            return _view.GetContent();
        }

        private void OnViewClosed(object sender, EventArgs e)
        {
            _singleWorker.Dispose();
            _logger.Dispose();
            _memoryTimer.Stop();
            _memoryTimer.Dispose();
        }

        private void OnRefreshStatus(object sender, EventArgs e)
        {
            UpdateRunningState();
        }

        private void UpdateMemoryStatus()
        {
            Process currentProcess = Process.GetCurrentProcess();
            long bytes = currentProcess.PrivateMemorySize64;
            string formatted = string.Format("Alkalmazás memóriahasználata: {0,12:F0} MB", bytes / 1024f / 1024f);
            _view.UpdateMemoryStatus(formatted);
        }

        private void UpdateTextboxMemoryStatus()
        {
            int chars = _logger.GetCurrentTextSize();
            double kb = chars * sizeof(char) / 1024.0;
            string formatted = string.Format("TextBox memóriahasználata: {0,18:F0} KB", kb);
            _view.UpdateTextboxMemoryStatus(formatted);
        }

        private bool IsValid(string value) =>
            Regex.IsMatch(value, @"^[A-Z]\d{4}$");

        private void LoggerLog(string value, int idx)
        {
            string suffix = idx == 0 ? "" : $" (#{idx})";
            string text = IsValid(value)
                ? $"{value} – MEGFELELŐ{suffix}"
                : $"{value} – NEM MEGFELELŐ{suffix}";
            _logger.AppendLine(text);
        }

        private void LoggerError(string error, int idx)
        {
            string suffix = idx == 0 ? "" : $" (#{idx})";
            _logger.AppendLine($"[HIBA]{suffix}: {error}");
        }
        private void OnGo()
        {
            try
            {

                if (!_singleWorker.Running || _view.IgnoreRunningState)
                {
                    _singleWorker.Start();
                    _logger.AppendLine(">> GO eljárás elindítva");
                }
                else
                {
                    _logger.AppendLine(">> Már fut a GO eljárás, újraindítás nem történt.");
                }
            }
            catch (Exception ex)
            {
                _logger.AppendLine("[HIBA] Start: " + ex.Message);
            }
        }

        private void OnStop()
        {
            try
            {
                if (_singleWorker == null)
                {
                    _logger?.AppendLine("[HIBA] NULLLLLLL");
                    return;
                }

                if (_singleWorker.Running || _view.IgnoreRunningState)
                {
                    _singleWorker.Stop();
                    _logger.AppendLine(">> STOP eljárás elindítva");
                }
                else
                {
                    _logger.AppendLine(">> A GO eljárás nem fut.");
                }
            }
            catch (Exception ex)
            {
                _logger.AppendLine("[HIBA] Stop: " + ex.Message);
            }
        }

        private async void OnSave()
        {
            try
            {
                var content = _view.GetContent();
                await _saveHandler.SaveLogAsync(content, msg => _logger.AppendLine(msg), silent: false);
            }
            catch (Exception ex)
            {
                _logger.AppendLine("[HIBA] Mentés: " + ex.Message);
            }
        }

        private void UpdateRunningState()
        {
            string singleText = _singleWorker.Running
                ? "Állapot: Futtatás alatt"
                : "Állapot: Leállítva";

            _view.UpdateSingleStatus(singleText);
        }
    }
}
