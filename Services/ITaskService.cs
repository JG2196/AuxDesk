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
        Task<TaskItem> GetTaskItemAsync(List<TaskItem> listTaskItems, string guid);
        Task DeleteTaskItemAsync(List<DeletedTaskItem> listDeletedTaskItems, TaskItem taskItemToDelete);
        Task<List<DeletedTaskItem>> GetDeletedTaskItemsAsync();
        Task UpdateTaskItemsAsync(List<TaskItem> listTaskItems);
        Task UpdateDeletedTaskItemsAsync(List<DeletedTaskItem> listDeletedTaskItems);
        Task SaveTaskItemAsync(List<TaskItem> listTaskItems, TaskItem taskItem);
        //Task<List<TaskItem>> FilterTask();
    }
}
