using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Services
{
    public interface ITimerService
    {
        void DisplayNotification(string message);
        //Task ResetTimer(bool bIsPomodoro);
        bool PauseTimer(System.Timers.Timer timer);
        void TimerCleanUp(System.Timers.Timer timer);
    }
}
