using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Reflection;
using System.Web;

namespace Common.API
{
    public interface IAPIService
    {
        Task<T> GetAsync<T>(string endpoint, object? request = null);
        Task<T> PostAsync<T>(string endpoint, object? request = null);
        Task<T> PutAsync<T>(string endpoint, object? request = null);
        Task<T> DeleteAsync<T>(string endpoint, object? request = null);
    }

    public class APIService : IAPIService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly Uri _baseUri;
        public MessageObject MessageObject { get; set; } = new();
        public Exception? Exception { get; set; }
        public bool Status { get; set; }

        public APIService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new();
            _baseUri = new Uri(_configuration.GetSection("Url").Value.ToString() ?? string.Empty);
        }

        public async Task<T> GetAsync<T>(string endpoint, object? request)
        {
            if (Uri.TryCreate(_baseUri, endpoint, out Uri requestUri))
            {
                var requestUrl = $"{requestUri}?{ToQueryString(request)}";
                try
                {
                    Status = true;
                    return await _httpClient.GetFromJsonAsync<T>(requestUrl);
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    Status = false;
                    return default;
                }
            }
            MessageObject.Code = "0000";
            Status = false;
            return default;
        }

        public async Task<T> PostAsync<T>(string endpoint, object? request)
        {
            if (Uri.TryCreate(_baseUri, endpoint, out Uri requestUri))
            {
                try
                {
                    var json = JsonConvert.SerializeObject(request);
                    var response = await _httpClient.PostAsJsonAsync(requestUri, request);
                    response.EnsureSuccessStatusCode();
                    Status = true;
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                catch (HttpRequestException ex)
                {
                    Exception = ex;
                    Status = false;
                    return default;
                }
            }
            MessageObject.Code = "0000";
            Status = false;
            return default;
        }

        public async Task<T> PutAsync<T>(string endpoint, object? request)
        {
            if (Uri.TryCreate(_baseUri, endpoint, out Uri requestUri))
            {
                try
                {
                    var response = await _httpClient.PutAsJsonAsync(requestUri, request);
                    response.EnsureSuccessStatusCode();
                    Status = true;
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                catch (HttpRequestException ex)
                {
                    Exception = ex;
                    Status = false;
                    return default;
                }
            }
            MessageObject.Code = "0000";
            Status = false;
            return default;
        }

        public async Task<T> DeleteAsync<T>(string endpoint, object? request)
        {
            if (Uri.TryCreate(_baseUri, endpoint, out Uri requestUri))
            {
                try
                {
                    var response = await _httpClient.PutAsJsonAsync(requestUri, request);
                    response.EnsureSuccessStatusCode();
                    Status = true;
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                catch (HttpRequestException ex)
                {
                    Exception = ex;
                    Status = false;
                    return default;
                }
            }
            MessageObject.Code = "0000";
            Status = false;
            return default;
        }

        private static string ToQueryString(object request)
        {
            if (request == null)
                return string.Empty;

            var properties = request.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Where(p => p.GetValue(request, null) != null)
                                .Select(p => $"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(p.GetValue(request, null).ToString())}");

            return string.Join("&", properties);
        }
    }
}
