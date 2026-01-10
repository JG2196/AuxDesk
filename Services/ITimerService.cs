using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Services
{
    using AuxDesk.Models;

    public interface ITimerService
    {
        Task<List<TaskItem>> GetTaskItemsAsync(DateOnly? selectedDate);
    }
}
