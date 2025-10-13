using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Data
{
    public class UserTask
    {
        public string TaskGUID { get; set; } = string.Empty;
        public DateOnly AssignedDate { get; set; }
        public DateTime? StartDateTime { get; set; } = null;
        public DateTime? EndDateTime { get; set; } = null;
        public int Priority { get; set; } = 0;
        public bool IsDone { get; set; } = false;
        public TaskData UserTaskData { get; set; } = new TaskData();
        public bool IsDeleted { get; set; } = false;
    }
    public class TaskData
    {
        public string TaskGUID { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
    public class DeletedTask
    {
        public string TaskGUID { get; set; } = string.Empty;
        public DateOnly? TaskDeletedDate { get; set; } = null;
    }
}
