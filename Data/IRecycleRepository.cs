using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Data
{
    using AuxDesk.Models;

    public interface IRecycleRepository
    {
        Task<List<DeletedTaskItem>> GetAllAsync();
        Task SaveAsync(List<DeletedTaskItem> listTaskItems);
        Task CreateFileAsync();
    }
}
