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
            List<UserTask> listUserTasks = GetUserTasks();

            DateOnly expireDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-14));

            listUserTasks = CRUDRemoveDeletedTasks(listUserTasks, expireDate);
            
            var expireDateTime = expireDate.ToDateTime(TimeOnly.MaxValue);
            listUserTasks = listUserTasks.Where(task => task.IsDone && task.EndDateTime < expireDateTime).ToList();

            Save(listUserTasks, null, false);
        }
        private List<UserTask> CRUDRemoveDeletedTasks(List<UserTask> listUserTasks, DateOnly expireDate)
        {
            List<DeletedTask> listDeletedTasks = GetDeletedTasks();
            if (listDeletedTasks.Count == 0) { return listUserTasks; }

            List<DeletedTask> listExpiredTasks = listDeletedTasks.Where(task => task.TaskDeletedDate < expireDate).ToList();
            if (listExpiredTasks.Count == 0) { return listUserTasks; }

            foreach (DeletedTask expiredTask in listExpiredTasks)
            {
                listUserTasks.RemoveAll(task => task.TaskGUID == expiredTask.TaskGUID);
                listDeletedTasks.RemoveAll(task => task.TaskGUID == expiredTask.TaskGUID);
            }

            Save(null, listDeletedTasks, true);
            return listUserTasks;
        }
    }
}
