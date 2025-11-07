using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Services
{
    using AuxDesk.Models;

    public interface ITaskService
    {
        Task<List<TaskItem>> GetTaskItemsAsync(DateOnly? selectedDate);
        Task<TaskItem> GetTaskItem(List<TaskItem> listTaksItems, string guid);
        //Task DeleteTaskItemAsync();
        //Task CreateTaskItemAsync();
        //Task<List<TaskItem>> FilterTask();
    }
}
