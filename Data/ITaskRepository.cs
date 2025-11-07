using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Data
{
    using AuxDesk.Models;

    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem> GetByIdAsync(string guid);
        Task AddAsync(TaskItem taskItem);
        Task SaveAsync(List<TaskItem> listTaskItems);
        Task UpdateAsync(TaskItem taskItem);
        Task DeleteAsync(string guid);
    }
}
