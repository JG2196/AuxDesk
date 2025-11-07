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

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
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
        public async Task<TaskItem> GetTaskItemAsync(string guid)
        {
            TaskItem listTaskItems = await _taskRepository.GetByIdAsync(guid);
            return listTaskItems;

        }
        //    public async Task DeleteTaskItemAsync() 
        //    {

        //    }
        //    public async Task CreateTaskItemAsync() 
        //    {

        //    }
        //    public Task<List<TaskItem>> FilterTask() 
        //    {

        //    }

    }
    }
