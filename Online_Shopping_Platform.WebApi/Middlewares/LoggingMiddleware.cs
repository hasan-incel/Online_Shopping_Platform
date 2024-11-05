namespace Online_Shopping_Platform.WebApi.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the incoming request's URL and timestamp
            _logger.LogInformation($"Request URL: {context.Request.Path} at {DateTime.Now}");

            // Pass control to the next middleware in the pipeline
            await _next(context);
        }
    }
}
