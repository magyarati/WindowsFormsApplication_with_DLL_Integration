using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication_with_DLL_Integration
{
    public class GetIDWorker : IDisposable
    {
        private readonly GetID.GetID _instance;
        private readonly PropertyChangedEventHandler _valueChangedHandler;
        private readonly PropertyChangedEventHandler _errorChangedHandler;

        public event EventHandler<string> ValueReceived;
        public event EventHandler<string> ErrorReceived;

        public bool Running => _instance.Running;
        public string Value => _instance.Value;
        public string ErrorMessage => _instance.ErrorMessage;

        public GetIDWorker()
        {
            _instance = new GetID.GetID();

            _valueChangedHandler = (s, e) =>
                ValueReceived?.Invoke(this, _instance.Value ?? "");
            _instance.ValueChanged += _valueChangedHandler;

            _errorChangedHandler = (s, e) =>
                ErrorReceived?.Invoke(this, _instance.ErrorMessage ?? "");
            _instance.ErrorChanged += _errorChangedHandler;
        }

        public void Start()
        {
            Task.Run(() => _instance.Go());
        }

        public void Stop()
        {
            _instance.Stop();
        }

        public void Dispose()
        {
            _instance.Stop();
            _instance.ValueChanged -= _valueChangedHandler;
            _instance.ErrorChanged -= _errorChangedHandler;
        }
    }
}
