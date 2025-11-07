using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Models
{
    public class TaskItem
    {
        public string TaskGUID { get; set; } = string.Empty;
        public DateOnly AssignedDate { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int Priority { get; set; }
        public bool IsDone { get; set; } = false;
        public string Title { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    }
}
