using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Services
{
    public interface ITimerService
    {
        event Action? Pomodoro_OnTimerTick;
        event Action? Pomodoro_OnTimerStateChanged;

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
