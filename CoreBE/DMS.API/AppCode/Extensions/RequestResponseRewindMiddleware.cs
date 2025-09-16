namespace DMS.API.AppCode.Extensions
{
    public class RequestBodyRewindMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                httpContext.Request.EnableBuffering();
                await _next(httpContext);
            }
            catch(Exception ex) {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            await ExceptionHandler.ExceptionResult(context, exception);
        }
    }

    public static class BodyRewindExtensions
    {
        public static IApplicationBuilder EnableRequestBodyRewind(this IApplicationBuilder app)
        {
            return app is null ? throw new ArgumentNullException(nameof(app)) : app.UseMiddleware<RequestBodyRewindMiddleware>();
        }
    }
}
