using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuxDesk.Data;

namespace AuxDesk.AuxDeskServices
{
    public partial class AuxDeskServices
    {
        public void CRUDPermenentDelete()
        {
            List<DeletedTask> listDeletedTasks = GetDeletedTasks();

            if (listDeletedTasks.Count == 0) { return; }

            List<UserTask> listUserTasks = GetUserTasks();

            DateOnly expireDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-14));
            List<DeletedTask> listExpiredTasks = listDeletedTasks.Where(task => task.TaskDeletedDate < expireDate).ToList();
            
            if (listExpiredTasks.Count == 0) { return; }

            foreach (DeletedTask expiredTask in listExpiredTasks)
            {
                listUserTasks.RemoveAll(task => task.TaskGUID == expiredTask.TaskGUID);
                listDeletedTasks.RemoveAll(task => task.TaskGUID == expiredTask.TaskGUID);
            }

            Save(listUserTasks, null, false);
            Save(null, listDeletedTasks, true);
        }
    }
}
