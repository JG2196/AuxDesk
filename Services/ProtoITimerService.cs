using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Services
{
    public interface ProtoITimerService
    {
        event Action? OnTimerTick;
        event Action? OnTimerStateChanged;

        bool bCountdownIsRunning();
        bool bPomodoroIsRunning();
        bool bCountdownIsPaused();
        int GetPresetTimer();
        void SetTimer(int selectedTime);
        string GetFormattedTime();
        void DisplayNotification(string message);
        void PauseTimer();
        void TimerCleanUp();
        void StartTimer();
        void StopTimer();
    }
}
