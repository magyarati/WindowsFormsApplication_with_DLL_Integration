using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication_with_DLL_Integration
{
    public class GetIDWorker : IDisposable
    {
        private readonly GetID.GetID _instance;

        public event EventHandler<string> ValueReceived;
        public event EventHandler<string> ErrorReceived;

        public bool Running => _instance.Running;

        public GetIDWorker()
        {
            _instance = new GetID.GetID();
            _instance.ValueChanged += (s, e) =>
                ValueReceived?.Invoke(this, _instance.Value ?? "");
            _instance.ErrorChanged += (s, e) =>
                ErrorReceived?.Invoke(this, _instance.ErrorMessage ?? "");
        }

        public void Start()
        {
            Task.Run(() => _instance.Go());
        }

        public void Stop()
        {
            if (Running)
                _instance.Stop();
        }

        public void Dispose()
        {
            Stop();
            _instance.Stop();
        }
    }
}
