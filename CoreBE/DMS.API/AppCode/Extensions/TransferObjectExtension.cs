using Newtonsoft.Json;
using DMS.API.AppCode.Util;
using Common;

namespace DMS.API.AppCode.Extensions
{
    public static class TransferObjectExtension
    {
        private static IHttpContextAccessor httpContextAccessor;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }

        public static void GetMessage(this TransferObject transferObject, string msgCode, IBaseService service, params object[] paramObject)
        {
            var logId = "LID" + DateTime.Now.ToString("yyMMddHHmmssFF");
            var code = msgCode;
            //if (!string.IsNullOrWhiteSpace(service.MessageObject.Code))
            //{
            //    code = service.MessageObject.Code;
            //}
            transferObject.MessageObject.Code = code;
            transferObject.MessageObject.Message = MessageUtil.GetMessage(code);
            if (!string.IsNullOrWhiteSpace(transferObject.MessageObject.Message))
            {
                transferObject.MessageObject.Message = string.Format(transferObject.MessageObject.Message, paramObject);
            }

            if (!string.IsNullOrWhiteSpace(service.MessageObject.Code))
            {
                transferObject.MessageObject.MessageDetail += MessageUtil.GetMessage(service.MessageObject.Code);
            }

            if (service.Exception != null)
            {
                transferObject.MessageObject.MessageDetail += service.Exception.Message;
                transferObject.MessageObject.LogId += logId;
            }

            //Ghi log
            if (!transferObject.Status || !service.Status)
            {
                var strRequest = string.Empty;
                try
                {
                    strRequest += $"....Method: {httpContextAccessor.HttpContext.Request.Method} {Environment.NewLine}";
                    strRequest += $"....Path: {httpContextAccessor.HttpContext.Request.Path} {Environment.NewLine}";
                    strRequest += $"....QueryString: {httpContextAccessor.HttpContext.Request.QueryString} {Environment.NewLine}";
                    strRequest += $"....Headers: {string.Join(System.Environment.NewLine, httpContextAccessor.HttpContext.Request.Headers.Keys.Select(key => key + ": " + string.Join(",", httpContextAccessor.HttpContext.Request.Headers[key])))} {Environment.NewLine}";
                    strRequest += $"....Schema: {httpContextAccessor.HttpContext.Request.Scheme} {Environment.NewLine}";
                    strRequest += $"....Host: {httpContextAccessor.HttpContext.Request.Host} {Environment.NewLine}";
                    strRequest += $"....Body: {httpContextAccessor.HttpContext.Request.ReadBodyFromRequest().Result} {Environment.NewLine}";

                    var messageLog = logId + Environment.NewLine +
                        JsonConvert.SerializeObject(transferObject, Formatting.Indented) + Environment.NewLine +
                        "ĐẦU VÀO HTTP REQUEST:" + Environment.NewLine +
                        strRequest + Environment.NewLine;


                    if (service.Exception != null)
                    {
                        messageLog += "EXCEPTION: " + Environment.NewLine + service.Exception.ToDetailedString();
                    }
                    LoggerService.LogError(messageLog);
                }
                catch (Exception ex)
                {
                    LoggerService.LogError("Có lỗi khi ghi file log:" + ex.ToString());
                }

            }
        }
    }
}
