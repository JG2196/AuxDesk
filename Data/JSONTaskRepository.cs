using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Data
{

    using AuxDesk.Models;
    using System.Text.Json;

    //using Windows.UI.Input.Inking.Analysis; Not sure why this was here in the first place

    public class JSONTaskRepository : ITaskRepository
    {
        private readonly string taskPath = Path.Combine(FileSystem.AppDataDirectory, "userTasks.json");
        //private readonly string deletedTaskPath = Path.Combine(FileSystem.AppDataDirectory, "recycleTask.json");

        public async Task<List<TaskItem>> GetAllAsync() 
        {
            List<TaskItem> listTaskItems = new List<TaskItem>();

            if (!File.Exists(taskPath)) { return listTaskItems; }

            var contents = File.ReadAllText(taskPath);

            var savedItems = JsonSerializer.Deserialize<List<TaskItem>>(contents);
            listTaskItems.AddRange(savedItems);
            listTaskItems = listTaskItems.OrderBy(task => task.Priority).ToList();

            return listTaskItems;
        }
        public async Task<TaskItem> GetByIdAsync(string guid) 
        {
            List<TaskItem> listTaskItems = await GetAllAsync();
            TaskItem taskItem = listTaskItems.FirstOrDefault(t => t.TaskGUID == guid);

            return taskItem;
        }
        public async Task AddAsync(TaskItem taskItem)
        {
            List<TaskItem> listTaskItems = await GetAllAsync();
            listTaskItems.Add(taskItem);
            await SaveAsync(listTaskItems);
        }
        public async Task SaveAsync (List<TaskItem> listTaskItems)
        {
            var contents = JsonSerializer.Serialize(listTaskItems);
            File.WriteAllText(taskPath, contents);
        }
        public async Task UpdateAsync(TaskItem taskItem) 
        {
            List<TaskItem> listTaskItems = await GetAllAsync();
            TaskItem itemToUpdate = listTaskItems.FirstOrDefault(t => t.TaskGUID == taskItem.TaskGUID);

            if (itemToUpdate != null)
            { 
                itemToUpdate = taskItem; 
            }

            await SaveAsync(listTaskItems);
        }
        public async Task DeleteAsync(string guid) 
        {
        
        }
    }
}
