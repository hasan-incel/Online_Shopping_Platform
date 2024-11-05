using Online_Shopping_Platform.Data.Entities;
using Online_Shopping_Platform.Data.Repositories;
using Online_Shopping_Platform.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Business.Operations.Setting
{
    public class SettingManager : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;  // Unit of work to handle transaction management
        private readonly IRepository<SettingEntity> _settingRepository;  // Repository to interact with settings in the database

        // Constructor to inject the unit of work and repository dependencies
        public SettingManager(IUnitOfWork unitOfWork, IRepository<SettingEntity> settingRepository)
        {
            _unitOfWork = unitOfWork;
            _settingRepository = settingRepository;
        }

        // Method to get the current maintenance mode state
        public bool GetMaintenanceState()
        {
            var maintenanceState = _settingRepository.GetById(1).MaintenanceMode;  // Fetch maintenance mode setting (ID=1)

            return maintenanceState;  // Return the current maintenance mode state
        }

        // Method to toggle the maintenance mode (enable/disable)
        public async Task ToggleMaintenance()
        {
            var setting = _settingRepository.GetById(1);  // Retrieve the setting entity by ID

            setting.MaintenanceMode = !setting.MaintenanceMode;  // Toggle the maintenance mode state

            _settingRepository.Update(setting);  // Update the setting entity in the repository

            try
            {
                await _unitOfWork.SaveChangesAsync();  // Save changes to the database
            }
            catch (Exception)
            {
                throw new Exception("There was an error while updating the maintenance status.");  // Handle errors during update
            }
        }
    }

}
