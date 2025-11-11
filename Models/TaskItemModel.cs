using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Models
{
    public class TaskItemData
    {
        public string TaskGUID { get; set; } = string.Empty;
        public int Priority { get; set; }
    }
    public class TaskItem : TaskItemData
    {
        public DateOnly? AssignedDate { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool IsDone { get; set; } = false;
        public string Title { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    
        public void Delete(bool bDelete)
        {
            IsDeleted = bDelete;
        }
        public bool NotStarted()
        {
            bool notStarted = true;

            if (StartDateTime != null)
            {
                notStarted = false;
            }

            return notStarted;
        }
        public bool InProgress()
        {
            bool inProgress = false;

            if (EndDateTime == null && StartDateTime != null)
            {
                inProgress = true;
            }

            return inProgress;
        }
        public bool IsCompleted()
        {
            return IsDone;
        }
        public bool ValidateTitle()
        {
            return string.IsNullOrWhiteSpace(Title);
        }
        public bool ValidateAssignedDate()
        {
            bool isValid = true;

            if (AssignedDate is null || AssignedDate < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                isValid = false;
            }

            return isValid;
        }
    }
    public class DeletedTaskItem : TaskItemData
    {
        public DateOnly? DateDeleted { get; set; }
    }

}
