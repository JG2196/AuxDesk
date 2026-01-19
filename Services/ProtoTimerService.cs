using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Services
{
    public class ProtoTimerService : ProtoITimerService
    {
        public event Action? OnTimerTick;
        public event Action? OnTimerStateChanged;

        private System.Timers.Timer? _timer;
        private TimeSpan _remainingTime;
        private bool _countdownIsRunning;
        private bool _pomodoroIsRunning;
        private bool _countdownIsPaused = false;
        private int _presetTimer = 1;

        public void StartTimer()
        {
            if (_countdownIsRunning && _countdownIsPaused)
            {
                _timer.Start();
                _countdownIsPaused = false;
            }
            else
            {
                _timer = new System.Timers.Timer(1000); // 1 second interval
                _timer.Elapsed += OnTimerElapsed;
                _timer.AutoReset = true;
                _timer.Start();
                _countdownIsRunning = true;
            }
            OnTimerStateChanged?.Invoke();
        }
        private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));

            if (_remainingTime.TotalSeconds <= 0)
            {
                StopTimer();
                DisplayNotification("Timer done");
            }

            OnTimerTick?.Invoke();
        }
        public void PauseTimer()
        {
            if (!_countdownIsRunning)
            {
                return;
            }

            if (_timer != null)
            {
                _timer.Stop();
                _countdownIsPaused = true;
            }
        }
        public void StopTimer()
        {
            _countdownIsRunning = false;

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= OnTimerElapsed;
                _timer.Dispose();
                _timer = null;
            }

            OnTimerStateChanged?.Invoke();
        }
        public bool bCountdownIsRunning() => _countdownIsRunning;
        public bool bPomodoroIsRunning() => _pomodoroIsRunning;
        public bool bCountdownIsPaused() => _countdownIsPaused;
        public int GetPresetTimer() => _presetTimer;
        public string GetFormattedTime()
        {
            return $"{_remainingTime.Hours:D2}:{_remainingTime.Minutes:D2}:{_remainingTime.Seconds:D2}";
        }
        public void SetTimer(int selectedDuration)
        {
            _presetTimer = selectedDuration;

            _remainingTime = selectedDuration switch
            {
                0 => TimeSpan.FromMinutes(15),
                1 => TimeSpan.FromMinutes(30),
                2 => TimeSpan.FromHours(1),
                _ => TimeSpan.FromMinutes(30)
            };
        }
        public void DisplayNotification(string message)
        {
            // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
            new ToastContentBuilder()
            //.AddArgument("action", "viewConversation")
            //.AddArgument("conversationId", 9813)
            .AddText(message)
            .Show(toast =>
            {
                toast.ExpirationTime = DateTime.Now.AddDays(1);
            });
            //.Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater
        }
        public void TimerCleanUp()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
