using Common;
using DMS.CORE;
using DMS.CORE.Entities.AD;

namespace DMS.API.Middleware
{
    public class ActionLoggingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (context.Request.Method == HttpMethods.Post ||
                    context.Request.Method == HttpMethods.Put ||
                    context.Request.Method == HttpMethods.Delete)
                {
                    // Log action here
                    var userId = context.User.Identity?.Name;
                    var actionName = context.Request.Path;
                    var controllerName = context.Request.RouteValues["controller"]?.ToString();

                    // Read the request body
                    context.Request.EnableBuffering();
                    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                    var requestTime = DateTime.UtcNow;
                    context.Request.Body.Position = 0;

                    // Capture the original response body stream
                    var originalResponseBodyStream = context.Response.Body;

                    using var responseBody = new MemoryStream();
                    context.Response.Body = responseBody;

                    await _next(context);

                    // Read the response body
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    var reponseTime = DateTime.UtcNow;
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var statusCode = context.Response.StatusCode;

                    var actionLog = new TblActionLog
                    {
                        UserName = userId,
                        ActionUrl = actionName,
                        RequestTime = requestTime,
                        RequestData = requestBody,
                        ResponseData = responseBodyText,
                        ResponseTime = reponseTime,
                        StatusCode = statusCode
                    };
                    // Lưu log vào cơ sở dữ liệu
                    var dbContext = context.RequestServices.GetService<AppDbContext>();
                    dbContext.TblActionLogs.Add(actionLog);
                    await dbContext.SaveChangesAsync();

                    // Copy the contents of the new memory stream (which contains the response) to the original stream.
                    await responseBody.CopyToAsync(originalResponseBodyStream);
                }
                else
                {
                    await _next(context);
                }
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex.Message);
                LoggerService.LogError(ex.StackTrace);
                await _next(context);
            }
        }
    }
}