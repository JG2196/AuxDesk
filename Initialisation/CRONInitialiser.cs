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
                // Get deleted tasks
                List<DeletedTaskItem> listRecycledTaskItems = await _recycleRepository.GetAllAsync();

                // If empty return
                if (listRecycledTaskItems.Count == 0) { return; }

                // Else get all tasks
                List<TaskItem> listTaskItems = await _taskRepository.GetAllAsync();

                // Set expire date (14 days before current date)
                DateOnly expireDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-14));

                // Filter deleted tasks to get expired tasks
                List<DeletedTaskItem> listExpiredTasks = listRecycledTaskItems.Where(task => task.DateDeleted < expireDate).ToList();
                if (listExpiredTasks.Count == 0) { return; }

                // Remove expired tasks from all tasks
                // Remove expired tasks from deleted tasks
                foreach (DeletedTaskItem expiredTask in listExpiredTasks)
                {
                    listTaskItems.RemoveAll(task => task.TaskGUID == expiredTask.TaskGUID);
                    listRecycledTaskItems.RemoveAll(task => task.TaskGUID == expiredTask.TaskGUID);
                }

                // Save files
                await _taskRepository.SaveAsync(listTaskItems);
                await _recycleRepository.SaveAsync(listRecycledTaskItems);
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
                List<TaskItem> listTaskItems = await _taskRepository.GetAllAsync();

                if (listTaskItems.Count == 0) { return; }

                if (listTaskItems.Any(taskItem => taskItem.IsDone))
                {
                    // Set expire date (31 days before current date)
                    DateOnly expireDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-31));

                    listTaskItems = listTaskItems.Where(task => DateOnly.FromDateTime((DateTime)task.EndDateTime) > expireDate).ToList();

                    await _taskRepository.SaveAsync(listTaskItems);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CleanupCompletedTasksAsync ex: {ex.Message}");
            }
        }
    }
}
