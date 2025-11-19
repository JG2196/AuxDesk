using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Data
{
    using AuxDesk.Models;
    using System.Text.Json;

    internal class JSONRecycleRepository : IRecycleRepository
    {
        private readonly string deletedTaskPath = Path.Combine(FileSystem.AppDataDirectory, "recycleTask.json");

        public async Task<List<DeletedTaskItem>> GetAllAsync()
        {
            List<DeletedTaskItem> listRecycledTaskItems = new List<DeletedTaskItem>();

            if (!File.Exists(deletedTaskPath)) 
            {
                //await CreateFileAsync();
                return listRecycledTaskItems; 
            }

            var contents = File.ReadAllText(deletedTaskPath);

            var savedItems = JsonSerializer.Deserialize<List<DeletedTaskItem>>(contents);
            listRecycledTaskItems.AddRange(savedItems);
            listRecycledTaskItems = listRecycledTaskItems.OrderBy(task => task.Priority).ToList();

            return listRecycledTaskItems;
        }
        public async Task SaveAsync(List<DeletedTaskItem> ListRecycledTaskItems)
        {
            var contents = JsonSerializer.Serialize(ListRecycledTaskItems);
            File.WriteAllText(deletedTaskPath, contents);
        }
        public async Task CreateFileAsync()
        {
            if (!File.Exists(deletedTaskPath))
            {
                await File.WriteAllTextAsync(deletedTaskPath, "[]");
            }
        }
    }
}
