using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using AuxDesk.Data;

namespace AuxDesk.AuxDeskServices
{
    public class AuxDeskServices
    {
        // FileSystem.AppDataDirectory - points to your app’s sandboxed data folder
        private readonly string taskPath = Path.Combine(FileSystem.AppDataDirectory, "userTasks.json");

        public List<UserTask> GetUserTasks()
        {
            List<UserTask> listUserTasks = new();

            if (!File.Exists(taskPath)) { return null; }

            var contents = File.ReadAllText(taskPath);
            var savedItems = JsonSerializer.Deserialize<List<UserTask>>(contents);
            listUserTasks.AddRange(savedItems);

            return listUserTasks;
        }
        public async Task Save(List<UserTask> listUserTasks)
        {
            var contents = JsonSerializer.Serialize(listUserTasks);

            File.WriteAllText(taskPath, contents);

            Console.WriteLine("List Saved", $"List has been saved to {taskPath}");
        }
    }

}
