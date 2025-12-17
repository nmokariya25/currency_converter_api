using Serilog;

namespace CurrencyConverter.Api.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestLoggingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var start = DateTime.UtcNow;
            await _next(context);
            var duration = DateTime.UtcNow - start;

            var clientId = context.User?.Claims.FirstOrDefault(c => c.Type == "clientId")?.Value;

            Log.Information("Request {Method} {Path} from {ClientIP} (ClientId: {ClientId}) responded {StatusCode} in {Duration}ms",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress,
                clientId,
                context.Response.StatusCode,
                duration.TotalMilliseconds);
        }
    }
}
