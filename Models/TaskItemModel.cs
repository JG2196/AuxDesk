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
        public string TaskGUID { get; set; }
        public int Priority { get; set; }
    }
    public class TaskItem : TaskItemData
    {
        //
        private DateOnly? assignedDate;
        private string? title;
        public DateTime? EndDateTime { get; private set; } = null;
        public DateTime? StartDateTime { get; private set; } = null;
        public bool IsDone { get; set; } = false;
        public string Notes { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateOnly? AssignedDate
        {
            get { return assignedDate; }
            set
            {
                if (value < DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    assignedDate = null;
                }
                else
                {
                    assignedDate = value;
                }
            }
        }
        public string? Title
        {
            get { return title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    title = null;
                }
                title = value;
            }
        }
        public void SetEndDateTime(DateTime date)
        {
            if (date < DateTime.UtcNow)
            {
                EndDateTime = null;
            }
            EndDateTime = date;
        }
        public void SetStartDateTime()
        {
            StartDateTime = DateTime.UtcNow;
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
    }
    public class DeletedTaskItem : TaskItemData
    {
        public DateOnly? DateDeleted { get; private set; }
        public void SetDateDeleted()
        {
            DateDeleted = DateOnly.FromDateTime(DateTime.UtcNow);
        }
    }

}
