using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Escant_App.BackgroundProcess
{
    public class Escant_AppTimer
    {
        private readonly TimeSpan _timespan;
        private readonly Action _callback;
        private CancellationTokenSource _cancellation;

        public Escant_AppTimer(TimeSpan timespan, Action callback)
        {
            _timespan = timespan;
            _callback = callback;
            _cancellation = new CancellationTokenSource();
        }

        public void Start()
        {
            CancellationTokenSource cts = _cancellation; // safe copy
            Device.StartTimer(_timespan, () =>
            {
                if (cts.IsCancellationRequested)
                    return false;
                _callback.Invoke();
                return true; // or true for periodic behavior
            });
        }

        public void Stop()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Cancel();
            Interlocked.Exchange(ref _cancellation, cts);
        }
    }
}
