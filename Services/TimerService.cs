using Microsoft.Toolkit.Uwp.Notifications;
using System.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Services
{
    public class TimerService : ITimerService
    {
        public async Task DisplayNotification(string message)
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
        //public async Task ResetTimer(bool bIsPomodoro)
        //{
        
        //}
        public bool PauseTimer(System.Timers.Timer timer)
        {
            bool bTimerIsRunning = true;

            if (timer != null && timer.Enabled)
            {
                timer.Stop();
                bTimerIsRunning = false;
            }

            return bTimerIsRunning;
        }
        public async Task TimerCleanUp(System.Timers.Timer timer)
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
