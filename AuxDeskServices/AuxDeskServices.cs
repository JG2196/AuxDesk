using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using AuxDesk.Data;

namespace AuxDesk.AuxDeskServices
{
    public partial class AuxDeskServices
    {
        // FileSystem.AppDataDirectory - points to your app’s sandboxed data folder
        private readonly string taskPath = Path.Combine(FileSystem.AppDataDirectory, "userTasks.json");
        private readonly string deletedTaskPath = Path.Combine(FileSystem.AppDataDirectory, "recycleTask.json");


        public List<UserTask> GetUserTasks()
        {
            List<UserTask> listUserTasks = new List<UserTask>();

            if (!File.Exists(taskPath)) { return listUserTasks; }

            var contents = File.ReadAllText(taskPath);
            
            var savedItems = JsonSerializer.Deserialize<List<UserTask>>(contents);
            listUserTasks.AddRange(savedItems);

            return listUserTasks;
        }
        public List<DeletedTask> GetDeletedTasks()
        {
            List<DeletedTask> listDeletedTasks = new List<DeletedTask>();

            if (!File.Exists(deletedTaskPath)) { return listDeletedTasks; }

            var contents = File.ReadAllText(deletedTaskPath);
        
            var savedItems = JsonSerializer.Deserialize<List<DeletedTask>>(contents);
            listDeletedTasks.AddRange(savedItems);

            return listDeletedTasks;
        }
        public void Save(List<UserTask>? listUserTasks, List<DeletedTask>? listDeletedTasks, bool setDeletedTasks)
        {
            var contents = string.Empty;

            if (listUserTasks != null)
            {
                contents = JsonSerializer.Serialize(listUserTasks);
            }
            else
            {
                contents = JsonSerializer.Serialize(listDeletedTasks);
            }

            if (!setDeletedTasks)
            {
                File.WriteAllText(taskPath, contents);
            }
            else
            {
                File.WriteAllText(deletedTaskPath, contents);
            }

            Console.WriteLine("List Saved", $"List has been saved to {taskPath}");
        }
        public void DeleteTask(DeletedTask deletedTask)
        {
            List<DeletedTask>? listDeletedTasks = GetDeletedTasks();

            if (listDeletedTasks == null)
            {
                listDeletedTasks = new List<DeletedTask>();
            }

            listDeletedTasks.Add(deletedTask);

            Save(null, listDeletedTasks, true);
        }
    }

}
