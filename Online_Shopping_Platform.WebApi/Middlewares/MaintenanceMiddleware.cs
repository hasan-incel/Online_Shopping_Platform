using Online_Shopping_Platform.Business.Operations.Setting;

namespace Online_Shopping_Platform.WebApi.Middlewares
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;

        // Constructor to initialize the next middleware in the pipeline
        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Middleware logic to check maintenance mode
        public async Task Invoke(HttpContext context)
        {
            // Retrieve the maintenance mode setting from the service
            var settingService = context.RequestServices.GetRequiredService<ISettingService>();
            bool maintenanceMode = settingService.GetMaintenanceState();

            // Allow login and settings API paths to bypass maintenance check
            if (context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/settings"))
            {
                await _next(context);  // Proceed with the next middleware
                return;
            }

            // If maintenance mode is on, return a maintenance message
            if (maintenanceMode)
            {
                await context.Response.WriteAsync("We are currently unable to provide service.");
            }
            else
            {
                await _next(context);  // Proceed with the next middleware if not in maintenance
            }
        }
    }

}
