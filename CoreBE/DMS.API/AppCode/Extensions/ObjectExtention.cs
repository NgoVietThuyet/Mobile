using DMS.COMMON.Common.Enum;

namespace DMS.API.AppCode.Extensions
{
    public static class ObjectExtension
    {
        public static string GetName(this object enumVal)
        {
            if (enumVal is System.Enum)
            {
                try
                {
                    var type = enumVal.GetType();
                    var memInfo = type.GetMember(enumVal.ToString());
                    var attributes = memInfo[0].GetCustomAttributes(typeof(EnumNameAttribute), false);
                    return (attributes.Length > 0) ? ((EnumNameAttribute)attributes[0]).Name : string.Empty;
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }

        public static string GetValue(this object enumVal)
        {
            if (enumVal is System.Enum)
            {
                try
                {
                    var type = enumVal.GetType();
                    var memInfo = type.GetMember(enumVal.ToString());
                    var attributes = memInfo[0].GetCustomAttributes(typeof(EnumValueAttribute), false);
                    return (attributes.Length > 0) ? ((EnumValueAttribute)attributes[0]).Value : string.Empty;
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }
    }
}
