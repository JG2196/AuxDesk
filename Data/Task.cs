using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Data
{
    public class UserTask
    {
        public string Title { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public int Priority { get; set; } = 0;
        public int Effort { get; set; } = 0;
        public bool IsDone { get; set; }
    }
}
