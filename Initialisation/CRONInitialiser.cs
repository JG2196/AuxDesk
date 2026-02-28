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

                // Clean up incomplete tasks
                await CleanupIncompleteTasksAsync();
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

                // SET listExpiredTasks to OUTPUT FilterDeletedTasks
                List<DeletedTaskItem> listExpiredTasks = FilterDeletedTasks(listRecycledTaskItems);

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
                // SET listAllTaskItems to OUTPUT of GetAllAsync
                List<TaskItem> listAllTaskItems = await _taskRepository.GetAllAsync();
                
                // SET listCompletedTaskItems to empty list
                List<TaskItem> listCompletedTaskItems = new List<TaskItem>();

                // IF listAllTaskItems CONTAINS any task WHERE IsDone IS true:
                if (listAllTaskItems.Any(taskItem => taskItem.IsDone))
                {
                    // SET listCompletedTaskItems to OUTPUT FilterTasks
                    listCompletedTaskItems = FilterTasks(true, listAllTaskItems);
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

        private async Task CleanupIncompleteTasksAsync()
        {
            try
            {
                // SET listAllTaskItems to OUTPUT of GetAllAsync
                List<TaskItem> listAllTaskItems = await _taskRepository.GetAllAsync();

                // SET listCompletedTaskItems to empty list
                List<TaskItem> listIncompletedTaskItems = new List<TaskItem>();

                // IF listAllTaskItems CONTAINS any task WHERE IsDone IS true:
                if (listAllTaskItems.Any(taskItem => !taskItem.IsDone))
                {
                    // SET listIncompletedTaskItems to OUTPUT FilterTasks
                    listIncompletedTaskItems = FilterTasks(false, listAllTaskItems);
                }
                // ELSE return
                else
                {
                    return;
                }
                // IF listCompletedTaskItems IS NOT empty:
                if (listIncompletedTaskItems.Count != 0)
                {
                    // FOR EACH task IN listCompletedTaskItems:
                    foreach (TaskItem task in listIncompletedTaskItems)
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

        private List<TaskItem> FilterTasks(bool isTaskComplete, List<TaskItem> listAllTaskItems)
        {
            List<TaskItem> listFilteredTaskItems = new List<TaskItem>();

            try
            {
                DateOnly expireDate = SetExpireDate(false);

                if (isTaskComplete)
                {
                    // SET listFilteredTaskItems to listAllTaskItems WHERE IsDone IS true AND EndDateTime IS NOT null AND EndDateTime < expiryDate
                    listFilteredTaskItems = listAllTaskItems.Where(task => task.IsDone && task.EndDateTime != null && DateOnly.FromDateTime((DateTime)task.EndDateTime) < expireDate).ToList();
                }
                else
                {
                    // SET listFilteredTaskItems to listAllTaskItems WHERE IsDone IS false AND IsDeleted IS false && AssignedDate IS < expiryDate
                    listFilteredTaskItems = listAllTaskItems.Where(task => !task.IsDone && !task.IsDeleted && task.AssignedDate < expireDate).ToList();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"FilterTasks ex: {ex.Message}");
            }

            return listFilteredTaskItems;
        }

        private List<DeletedTaskItem> FilterDeletedTasks(List<DeletedTaskItem> listRecycledTaskItems)
        {
            List<DeletedTaskItem> listExpiredTasks = new List<DeletedTaskItem>();

            try
            {
                DateOnly expireDate = SetExpireDate(true);

                // SET listExpiredTasks to listRecycledTaskItems WHERE DateDeleted < expireDate
                listExpiredTasks = listRecycledTaskItems.Where(task => task.DateDeleted < expireDate).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FilterDeletedTasks ex: {ex.Message}");
            }

            return listExpiredTasks;
        }

        private DateOnly SetExpireDate(bool isTaskDeleted)
        {
            int days = isTaskDeleted ? -14 : -31;

            DateOnly expireDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(days));

            return expireDate;
        }
    }
}
