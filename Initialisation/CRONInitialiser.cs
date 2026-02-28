using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Initialisation
{
    using AuxDesk.Data;
    using AuxDesk.Models;

    internal class CRONInitialiser
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IRecycleRepository _recycleRepository;

        public CRONInitialiser(ITaskRepository taskRepository, IRecycleRepository recycleRepository)
        {
            _taskRepository = taskRepository;
            _recycleRepository = recycleRepository;
        }

        public async Task CRONInitialiseAsync()
        {
            try
            {
                // Clean up recycle bin
                await CleanupRecycleBinAsync();

                // Clean up completed tasks
                await CleanupCompletedTasksAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRONInitialiseAsync ex: {ex.Message}");
            }
        }

        private async Task CleanupRecycleBinAsync()
        {
            try
            {
                // Set expireDate to 14 days before current date
                DateOnly expireDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-14));

                // SET listRecycledTaskItems to OUTPUT of GetAllAsync
                List<DeletedTaskItem> listRecycledTaskItems = await _recycleRepository.GetAllAsync();

                // IF listRecycledTaskItems IS empty: return
                if (listRecycledTaskItems.Count == 0) { return; }

                // SET listExpiredTasks to listRecycledTaskItems WHERE DateDeleted < expireDate
                List<DeletedTaskItem> listExpiredTasks = listRecycledTaskItems.Where(task => task.DateDeleted < expireDate).ToList();
                
                // IF listExpiredTasks IS NOT empty:
                if (listExpiredTasks.Count != 0)
                {
                    // SET listTaskItems to OUTPUT of GetAllAsync
                    List<TaskItem> listTaskItems = await _taskRepository.GetAllAsync();

                    // FOR EACH expiredTask IN listExpiredTasks:
                    foreach (DeletedTaskItem expiredTask in listExpiredTasks)
                    {
                        // REMOVE expiredTask FROM listTaskItems
                        listTaskItems.RemoveAll(task => task.TaskGUID == expiredTask.TaskGUID);
                        // REMOVE expiredTask FROM listRecycledTaskItems
                        listRecycledTaskItems.RemoveAll(task => task.TaskGUID == expiredTask.TaskGUID);
                    }

                    // CALL SaveAsync with listTaskItems AND listRecycledTaskItems
                    await _taskRepository.SaveAsync(listTaskItems);
                    await _recycleRepository.SaveAsync(listRecycledTaskItems);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CleanupRecycleBinAsync ex: {ex.Message}");
            }
        }

        private async Task CleanupCompletedTasksAsync()
        {
            try
            {
                // SET expiryDate to 31 days before current date
                DateOnly expireDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-31));
                
                // SET listAllTaskItems to OUTPUT of GetAllAsync
                List<TaskItem> listAllTaskItems = await _taskRepository.GetAllAsync();
                
                // SET listCompletedTaskItems to empty list
                List<TaskItem> listCompletedTaskItems = new List<TaskItem>();

                // IF listAllTaskItems CONTAINS any task WHERE IsDone IS true:
                if (listAllTaskItems.Any(taskItem => taskItem.IsDone))
                {
                    // SET listCompletedTaskItems to listAllTaskItems WHERE IsDone IS true AND EndDateTime IS NOT null AND EndDateTime < expiryDate
                    listCompletedTaskItems = listAllTaskItems.Where(task => task.IsDone && task.EndDateTime != null && DateOnly.FromDateTime((DateTime)task.EndDateTime) < expireDate).ToList();
                }
                // ELSE return
                else
                {
                    return;
                }
                // IF listCompletedTaskItems IS NOT empty:
                if (listCompletedTaskItems.Count != 0)
                {
                    // FOR EACH task IN listCompletedTaskItems:
                    foreach (TaskItem task in listCompletedTaskItems)
                    {
                        // REMOVE task FROM listAllTaskItems
                        listAllTaskItems.RemoveAll(t => t.TaskGUID == task.TaskGUID);
                    }
                    // CALL SaveAsync with listAllTaskItems
                    await _taskRepository.SaveAsync(listAllTaskItems);
                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"CleanupCompletedTasksAsync ex: {ex.Message}");
            }
        }
    }
}
