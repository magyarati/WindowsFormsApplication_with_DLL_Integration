using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApplication_with_DLL_Integration
{
    class MainPresenter
    {
        private readonly IMainView _view;
        private readonly ISaveHandler _saveHandler;
        private readonly BatchingLogger _logger;

        private readonly GetIDWorker _singleWorker;
        private readonly List<GetIDWorker> _multiWorkers = new List<GetIDWorker>();

        public MainPresenter(IMainView view,
                             ISaveHandler saveHandler,
                             BatchingLogger logger)
        {
            _view = view;
            _saveHandler = saveHandler;
            _logger = logger;

            _singleWorker = new GetIDWorker();
            _singleWorker.ValueReceived += (s, v) => LoggerLog(v, 0);
            _singleWorker.ErrorReceived += (s, e) => LoggerError(e, 0);

            _view.GoRequested += (sender, e) => OnGo();
            _view.StopRequested += (sender, e) => OnStop();
            _view.SaveRequested += (sender, e) => OnSave();
            _view.MultiGoRequested += (sender, e) => OnMultiGo();
            _view.MultiStopRequested += (sender, e) => OnMultiStop();

            UpdateRunningState();
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
                    UpdateRunningState();
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
                if (_singleWorker.Running || _view.IgnoreRunningState)
                {
                    _singleWorker.Stop();
                    _logger.AppendLine(">> STOP eljárás elindítva");
                    UpdateRunningState();
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
                await _saveHandler.SaveLogAsync(
                    () => _logger.GetContent(),
                    msg => _logger.AppendLine(msg),
                    silent: false);
            }
            catch (Exception ex)
            {
                _logger.AppendLine("[HIBA] Mentés: " + ex.Message);
            }
        }

        private void OnMultiGo()
        {
            try
            {
                Parallel.ForEach(_multiWorkers, w => w.Dispose());
                _multiWorkers.Clear();

                int darab = _view.MultiGetIDsValue;
                if (darab < 1)
                {
                    _logger.AppendLine(">> MultiGo: érvénytelen darabszám.");
                    return;
                }

                for (int i = 0; i < darab; i++)
                {
                    var worker = new GetIDWorker();
                    int idx = i + 1;
                    worker.ValueReceived += (s, v) => LoggerLog(v, idx);
                    worker.ErrorReceived += (s, err) => LoggerError(err, idx);
                    _multiWorkers.Add(worker);
                }

                Parallel.For(0, _multiWorkers.Count, i =>
                {
                    _multiWorkers[i].Start();
                    _logger.AppendLine($">> Multi GO eljárás indítva: példány #{i + 1}");
                });

                _logger.AppendLine($">> Összesen {_multiWorkers.Count} példány fut.");
                UpdateRunningState();
            }
            catch (Exception ex)
            {
                _logger.AppendLine("[HIBA] MultiGo: " + ex.Message);
            }
        }

        private void OnMultiStop()
        {
            try
            {
                if (_multiWorkers.Count == 0)
                {
                    _logger.AppendLine(">> Nincsenek futó Multi-példányok.");
                    return;
                }

                Parallel.ForEach(_multiWorkers, w => w.Dispose());
                _logger.AppendLine($">> Leállítva {_multiWorkers.Count} multi-példány.");
                _multiWorkers.Clear();
                UpdateRunningState();
            }
            catch (Exception ex)
            {
                _logger.AppendLine("[HIBA] MultiStop: " + ex.Message);
            }
        }

        private void UpdateRunningState()
        {
            string singleText = _singleWorker.Running
                ? "Állapot: Futtatás alatt"
                : "Állapot: Leállítva";

            int multiRunning = _multiWorkers.Count(w => w.Running);
            string multiText = $"Állapot: {multiRunning} szál fut";

            _view.UpdateSingleStatus(singleText);
            _view.UpdateMultiStatus(multiText);
        }

        internal void OnClosing()
        {
            _singleWorker.Dispose();
            foreach (var w in _multiWorkers)
                w.Dispose();
            _logger.Dispose();
        }
    }
}
