using System.Text;

namespace DMS.API.AppCode.Extensions
{
    public static class HttpRequestExtensions
    {
        public static async Task<string> ReadBodyFromRequest(this HttpRequest request, Encoding? encoding = null, int maxCharacters = 1000)
        {
            try
            {
                request.Body.Position = 0;
                using var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);

                // Read up to the specified maximum number of characters
                var buffer = new char[maxCharacters];
                var bytesRead = await reader.ReadAsync(buffer, 0, maxCharacters).ConfigureAwait(false);

                // Check if any characters were read
                if (bytesRead > 0)
                {
                    // Convert the character array to a string and return it
                    var body = new string(buffer, 0, bytesRead);
                    request.Body.Position = 0;
                    return body;
                }
                else
                {
                    // No characters were read
                    request.Body.Position = 0;
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return "Exception: Không đọc được thông tin body từ Request: " + ex.ToString();
            }
        }

    }
}
