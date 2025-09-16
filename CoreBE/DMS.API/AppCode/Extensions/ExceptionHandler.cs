using Microsoft.AspNetCore.Mvc;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Util;
using Newtonsoft.Json;
using Common;

namespace DMS.API.AppCode.Extensions
{
    public static class ExceptionHandler
    {
        public static async Task ExceptionResult(HttpContext context, Exception exception)
        {
            var logId = "LID" + DateTime.Now.ToString("yyMMddHHmmssFF");
            var transferObject = new TransferObject()
            {
                Status = false,
                MessageObject = new MessageObject()
                {
                    Code = "0001",
                    MessageType = MessageType.Error,
                    Message = MessageUtil.GetMessage("0001"),
                    MessageDetail = exception.Message,
                    LogId = logId
                }
            };

            var strRequest = string.Empty;
            try
            {
                strRequest += $"....Method: {context.Request.Method} {Environment.NewLine}";
                strRequest += $"....Path: {context.Request.Path} {Environment.NewLine}";
                strRequest += $"....QueryString: {context.Request.QueryString} {Environment.NewLine}";
                strRequest += $"....Headers: {string.Join(System.Environment.NewLine, context.Request.Headers.Keys.Select(key => key + ": " + string.Join(",", context.Request.Headers[key])))} {Environment.NewLine}";
                strRequest += $"....Schema: {context.Request.Scheme} {Environment.NewLine}";
                strRequest += $"....Host: {context.Request.Host} {Environment.NewLine}";
                strRequest += $"....Body: {context.Request.ReadBodyFromRequest().Result} {Environment.NewLine}";

                var messageLog = logId + Environment.NewLine +
                        JsonConvert.SerializeObject(transferObject, Formatting.Indented) + Environment.NewLine +
                        "ĐẦU VÀO HTTP REQUEST:" + Environment.NewLine +
                        strRequest + Environment.NewLine;

                messageLog += "EXCEPTION: " + Environment.NewLine + exception.ToDetailedString();

                LoggerService.LogError(messageLog);
            }
            catch (Exception ex)
            { 
                LoggerService.LogError("Có lỗi khi ghi file log:" + ex.ToDetailedString());
            }

            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(transferObject);
        }

        public static OkObjectResult ExceptionValidationResult(ActionContext context)
        {
            var logId = "LID" + DateTime.Now.ToString("yyMMddHHmmssFF");
            var transferObject = new TransferObject()
            {
                Status = false,
                MessageObject = new MessageObject()
                {
                    Code = "0000",
                    MessageType = MessageType.Error,
                    Message = MessageUtil.GetMessage("0000"),
                    MessageDetail = string.Join("; ", context.ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage)),
                    LogId = logId
                }
            };

            var strRequest = string.Empty;
            try
            {
                strRequest += $"....Method: {context.HttpContext.Request.Method} {Environment.NewLine}";
                strRequest += $"....Path: {context.HttpContext.Request.Path} {Environment.NewLine}";
                strRequest += $"....ActionController: {context.ActionDescriptor.DisplayName} {Environment.NewLine}";
                strRequest += $"....QueryString: {context.HttpContext.Request.QueryString} {Environment.NewLine}";
                strRequest += $"....Headers: {context.HttpContext.Request.Headers} {Environment.NewLine}";
                strRequest += $"....Headers: {string.Join(System.Environment.NewLine, context.HttpContext.Request.Headers.Keys.Select(key => key + ": " + string.Join(",", context.HttpContext.Request.Headers[key])))} {Environment.NewLine}";
                strRequest += $"....Schema: {context.HttpContext.Request.Scheme} {Environment.NewLine}";
                strRequest += $"....Host: {context.HttpContext.Request.Host} {Environment.NewLine}";
                strRequest += $"....Body: {context.HttpContext.Request.ReadBodyFromRequest().Result} {Environment.NewLine}";
            }
            catch (Exception ex)
            {
                LoggerService.LogError("Có lỗi khi ghi file log:" + ex.ToDetailedString());
            }


            LoggerService.LogError(
                logId + Environment.NewLine +
                JsonConvert.SerializeObject(transferObject, Formatting.Indented) + Environment.NewLine +
                "ĐẦU VÀO HTTP REQUEST:" + Environment.NewLine +
                strRequest);

            return new OkObjectResult(transferObject);
        }
    }
}
