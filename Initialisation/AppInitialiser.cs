using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxDesk.Initialisation
{
    using AuxDesk.Data;
    //using AuxDesk.Models;

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
            try
            {
                // Check if files exist, if not create files
                await ValidateExistingFiles();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"InitialiseAsync ex: {ex.Message}");
            }
        }
        
        private async Task ValidateExistingFiles()
        {
            try
            {
                await _taskRepository.CreateFileAsync();
                await _recycleRepository.CreateFileAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ValidateExistingFiles ex: {ex.Message}");
            }
        }
    }
}
