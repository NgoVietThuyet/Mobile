using Common;
using DMS.API.AppCode.Cache;

namespace DMS.API.AppCode.Util
{
    public static class MessageUtil
    {
        public static string GetMessage(string code, string lang = "")
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            if (string.IsNullOrWhiteSpace(lang))
            {
                lang = configuration.GetValue<string>("LanguageDefault");   
            }

            var applicationName = configuration.GetValue<string>("ApplicationName");
            var strKey = $"{applicationName}-Message-{code}-{lang}";
            return CachingProvider.GetItem(strKey) is MessageObject obj ? obj.Message : code;
        }

        public static void AddToCache(MessageObject obj)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var applicationName = configuration.GetValue<string>("ApplicationName");
            var strKey = $"{applicationName}-Message-{obj.Code}-{obj.Language}";
            CachingProvider.AddItem(strKey, obj);
        }
    }
}
