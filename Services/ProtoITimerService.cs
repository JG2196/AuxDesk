using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Services
{
    public interface ProtoITimerService
    {
        event Action? Countdown_OnTimerTick;
        event Action? Countdown_OnTimerStateChanged;

        event Action? Pomodoro_OnTimerTick;
        event Action? Pomodoro_OnTimerStateChanged;

        bool Countdown_IsRunning();
        bool Countdown_IsPaused();
        int Countdown_GetPresetTimer();
        void Countdown_StartTimer();
        void Countdown_PauseTimer();
        void Countdown_StopTimer();
        //
        bool Pomodoro_IsRunning();
        bool Pomodoro_IsPaused();
        int Pomodoro_GetPomodoroType();
        int Pomodoro_GetSetCycles();
        int Pomodoro_GetCompletedCycles();
        void Pomodoro_StartTimer(int pomodoroCycles);
        void Pomodoro_PauseTimer();
        void Pomodoro_StopTimer();
        void Pomodoro_ResetTimer();




        void SetTimer(int selectedTime);
        string GetFormattedTime();
        void DisplayNotification(string message);
        void TimerCleanUp();

    }
}
