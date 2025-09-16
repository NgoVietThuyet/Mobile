using System.Security.Cryptography;

namespace Common.Util
{
    public static class Utils
    {
        public static string CryptographyMD5(string source)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] bytHash = MD5.HashData(buffer);
            string result = string.Empty;
            foreach (byte a in bytHash)
            {
                result += int.Parse(a.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
            }
            return result;
        }


        public static IEnumerable<DateTime> LoopDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
