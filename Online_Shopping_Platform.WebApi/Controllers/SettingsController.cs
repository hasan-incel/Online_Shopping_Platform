using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Shopping_Platform.Business.Operations.Setting;

namespace Online_Shopping_Platform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;

        // Constructor to inject ISettingService dependency
        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        // Endpoint to toggle maintenance mode, only accessible by Admin role
        [HttpPatch]  // HTTP method to update partial data
        [Authorize(Roles = "Admin")]  // Restrict access to Admin role
        public async Task<IActionResult> ToggleMaintenance()
        {
            // Call service to toggle maintenance mode
            await _settingService.ToggleMaintenance();

            // Return success response
            return Ok();
        }
    }
}
