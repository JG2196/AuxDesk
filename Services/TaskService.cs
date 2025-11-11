using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Services
{
    using AuxDesk.Data;
    using AuxDesk.Models;

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IRecycleRepository _recycleRepository;

        public TaskService(ITaskRepository taskRepository, IRecycleRepository recycleRepository)
        {
            _taskRepository = taskRepository;
            _recycleRepository = recycleRepository;
        }

        public async Task<List<TaskItem>> GetTaskItemsAsync(DateOnly? selectedDate)
        {
            // Get all tasks
            List<TaskItem> listTaskItems = await _taskRepository.GetAllAsync();
            
            // Check if date is defined
            if (selectedDate != null)
            {
                // if date filter by date
                listTaskItems = listTaskItems.Where(t => t.AssignedDate == selectedDate).ToList();
                return listTaskItems;
            }
            else
            {
                // if no date return all
                return listTaskItems;
            }
        }
        public async Task<TaskItem> GetTaskItemAsync(List<TaskItem> listTaskItems, string guid)
        {
            TaskItem taskItem = listTaskItems.FirstOrDefault(t => t.TaskGUID == guid);
            return taskItem;

        }
        public async Task<List<DeletedTaskItem>> GetDeletedTaskItemsAsync() 
        {
            // Get all tasks
            List<DeletedTaskItem> listFeletedTaskItems = await _recycleRepository.GetAllAsync();

            return listFeletedTaskItems;
        }
        public async Task DeleteTaskItemAsync(List<DeletedTaskItem> listDeletedTaskItems, TaskItem taskItemToDelete)
        {
            listDeletedTaskItems.Add(new DeletedTaskItem
            {
                TaskGUID = taskItemToDelete.TaskGUID,
                DateDeleted = DateOnly.FromDateTime(DateTime.UtcNow)
            });

            await _recycleRepository.SaveAsync(listDeletedTaskItems);
        }
        public async Task UpdateTaskItemsAsync(List<TaskItem> listTaskItems)
        {
            await _taskRepository.SaveAsync(listTaskItems);
        }
        public async Task UpdateDeletedTaskItemsAsync(List<DeletedTaskItem> listDeletedTaskItems)
        {
            await _recycleRepository.SaveAsync(listDeletedTaskItems);
        }
        public async Task SaveTaskItemAsync(List<TaskItem> listTaskItems, TaskItem taskItem)
        {
            listTaskItems.Add(taskItem);
            await _taskRepository.SaveAsync(listTaskItems);
        }
        //    public Task<List<TaskItem>> FilterTask() 
        //    {

        //    }

    }
    }
