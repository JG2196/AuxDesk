using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Data
{

    using AuxDesk.Models;
    using System.Text.Json;

    public class JSONTaskRepository : ITaskRepository
    {
        private readonly string taskPath = Path.Combine(FileSystem.AppDataDirectory, "userTasks.json");

        public async Task<List<TaskItem>> GetAllAsync() 
        {
            List<TaskItem> listTaskItems = new List<TaskItem>();

            if (!File.Exists(taskPath)) {
                //await CreateFileAsync();
                return listTaskItems; 
            }

            var contents = File.ReadAllText(taskPath);

            var savedItems = JsonSerializer.Deserialize<List<TaskItem>>(contents);
            listTaskItems.AddRange(savedItems);
            listTaskItems = listTaskItems.OrderBy(task => task.Priority).ToList();

            return listTaskItems;
        }
        public async Task SaveAsync (List<TaskItem> listTaskItems)
        {
            var contents = JsonSerializer.Serialize(listTaskItems);
            File.WriteAllText(taskPath, contents);
        }
        public async Task CreateFileAsync()
        {
            if (!File.Exists(taskPath))
            {
                await File.WriteAllTextAsync(taskPath, "[]");
            }
        }
    }
}
