using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Initialisation
{
    using AuxDesk.Data;
    using AuxDesk.Models;

    internal class AppInitialiser
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IRecycleRepository _recycleRepository;

        public AppInitialiser(ITaskRepository taskRepository, IRecycleRepository recycleRepository)
        {
            _taskRepository = taskRepository;
            _recycleRepository = recycleRepository;
        }

        public async Task InitialiseAsync()
        {
            Console.WriteLine("Initializing application...");

            // 1. Seed default data
            //await SeedDefaultTasksAsync();

            // 2. Clean up recycle bin
            //await CleanupRecycleBinAsync();

            // 3. Validate data integrity
            //await ValidateDataAsync();

            Console.WriteLine("Application initialized successfully!");
        }
    }
}
