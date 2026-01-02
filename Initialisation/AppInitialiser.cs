using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Initialisation
{
    using AuxDesk.Data;
    using AuxDesk.Models;

    internal class AppInitialiser
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IRecycleRepository _recycleRepository;

        public AppInitialiser(ITaskRepository taskRepository, IRecycleRepository recycleRepository)
        {
            _taskRepository = taskRepository;
            _recycleRepository = recycleRepository;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                // 1. Check if files exist, if not create files
                await ValidateExistingFiles();

                // 2. Clean up recycle bin
                await CleanupRecycleBinAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"InitialiseAsync ex: {ex.Message}");
            }
        }
        
        private async Task ValidateExistingFiles()
        {
            try
            {
                await _taskRepository.CreateFileAsync();
                await _recycleRepository.CreateFileAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ValidateExistingFiles ex: {ex.Message}");
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
    }
}
