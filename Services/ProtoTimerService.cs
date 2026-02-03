using Microsoft.Maui.Controls;
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
        public event Action? Countdown_OnTimerTick;
        public event Action? Countdown_OnTimerStateChanged;

        public event Action? Pomodoro_OnTimerTick;
        public event Action? Pomodoro_OnTimerStateChanged;

        private System.Timers.Timer? _timer;
        private TimeSpan _remainingTime;
        private bool _countdownIsRunning;
        private bool _pomodoroIsRunning;
        private bool _countdownIsPaused = false;
        private bool _pomodoroIsPaused = false;
        private int _pomodoroType = 3;
        private int _presetTimer = 1;

        private int pomodoroCycles;
        private int selectedCycles = 4;
        bool bOnBreak = false;

        public void Countdown_StartTimer()
        {
            if (_countdownIsRunning && _countdownIsPaused)
            {
                _timer.Start();
                _countdownIsPaused = false;
            }
            else
            {
                _timer = new System.Timers.Timer(1000); // 1 second interval
                _timer.Elapsed += Countdown_OnTimerElapsed;
                _timer.AutoReset = true;
                _timer.Start();
                _countdownIsRunning = true;
            }
            Countdown_OnTimerStateChanged?.Invoke();
        }
        private void Countdown_OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));

            if (_remainingTime.TotalSeconds <= 0)
            {
                Countdown_StopTimer();
                DisplayNotification("Timer done");
            }

            Countdown_OnTimerTick?.Invoke();
        }
        public void Countdown_PauseTimer()
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
        public void Countdown_StopTimer()
        {
            _countdownIsRunning = false;
            SetTimer(_presetTimer);
            _countdownIsPaused = false;

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= Countdown_OnTimerElapsed;
                _timer.Dispose();
                _timer = null;
            }

            Countdown_OnTimerStateChanged?.Invoke();
        }
        public bool Countdown_IsRunning() => _countdownIsRunning;
        public bool Countdown_IsPaused() => _countdownIsPaused;
        public int Countdown_GetPresetTimer() => _presetTimer;
        
        public void SetTimer(int selectedDuration)
        {
            _presetTimer = selectedDuration;

            _remainingTime = selectedDuration switch
            {
                0 => TimeSpan.FromMinutes(15),
                //1 => TimeSpan.FromMinutes(30),
                2 => TimeSpan.FromHours(1),
                //3 => TimeSpan.FromMinutes(5),
                //4 => TimeSpan.FromMinutes(25),
                3 => TimeSpan.FromSeconds(3),
                4 => TimeSpan.FromSeconds(3),
                1 => TimeSpan.FromSeconds(3),
                _ => TimeSpan.FromMinutes(30)
            };
        }
        //
        public bool Pomodoro_IsRunning() => _pomodoroIsRunning;
        public bool Pomodoro_IsPaused() => _pomodoroIsPaused;
        public int Pomodoro_GetPomodoroType() => _pomodoroType;
        public int Pomodoro_GetSetCycles() => selectedCycles;
        public int Pomodoro_GetCompletedCycles() => pomodoroCycles;
        public void Pomodoro_StartTimer(int pomodoroCycles)
        {
            if (_pomodoroIsRunning && _pomodoroIsPaused)
            {
                _timer.Start();
                _pomodoroIsPaused = false;
            }
            else
            {
                _pomodoroType = 0;
                selectedCycles = pomodoroCycles;
                _timer = new System.Timers.Timer(1000); // 1 second interval
                _timer.Elapsed += Pomodoro_OnTimerElapsed;
                _timer.AutoReset = true;
                _timer.Start();
                _pomodoroIsRunning = true;
            }
            Pomodoro_OnTimerStateChanged?.Invoke();
        }
        private void Pomodoro_OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));

            string message = "";

            if (pomodoroCycles >= selectedCycles)
            {
                _remainingTime = TimeSpan.Zero;
                Pomodoro_StopTimer();
                Pomodoro_ResetTimer();
                DisplayNotification("Cycles completed");
                _pomodoroType = 3;
            }

            else if (_remainingTime.TotalSeconds == 0 && bOnBreak)
            {
                bOnBreak = false;
                SetTimer(4);
                _pomodoroType = 0;
                DisplayNotification("Break over! Time to work.");
            }
            else if (_remainingTime.TotalSeconds == 0 && !bOnBreak)
            {
                // Timer finished
                pomodoroCycles++;
                bOnBreak = true;
                if (pomodoroCycles % 4 == 0 && pomodoroCycles != selectedCycles)
                {
                    SetTimer(1);
                    _pomodoroType = 2;
                    message = "Work session over! Time for a long break.";
                }
                else if (pomodoroCycles != selectedCycles)
                {
                    SetTimer(3);
                    _pomodoroType = 1;
                    message = "Work session over! Time for a short break.";
                }
                if (pomodoroCycles < selectedCycles)
                {
                    DisplayNotification(message);
                }
            
            }
            Pomodoro_OnTimerTick?.Invoke();
        }
        public void Pomodoro_PauseTimer()
        {
            if (!_pomodoroIsRunning)
            {
                return;
            }

            if (_timer != null)
            {
                _timer.Stop();
                _pomodoroIsPaused = true;
            }
        }
        public void Pomodoro_StopTimer()
        {
            _pomodoroIsRunning = false;

            if (_timer != null)
            {
                pomodoroCycles = 0;
                _timer.Stop();
                _timer.Elapsed -= Pomodoro_OnTimerElapsed;
                _timer.Dispose();
                _timer = null;
            }

            Pomodoro_OnTimerStateChanged?.Invoke();
        }
        public void Pomodoro_ResetTimer()
        {
            _remainingTime = TimeSpan.Zero;
            _pomodoroIsRunning = false;
            _pomodoroIsPaused = false;
            _pomodoroType = 0;
            _presetTimer = 1;

            pomodoroCycles = 0;
            bOnBreak = false;
        }
        //
        public string GetFormattedTime()
        {
            return $"{_remainingTime.Hours:D2}:{_remainingTime.Minutes:D2}:{_remainingTime.Seconds:D2}";
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
