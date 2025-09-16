using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DMS.API.Controllers.CM
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeepSeekController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;
        public DeepSeekController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet("ChatDeepSeek")]
        public async IAsyncEnumerable<string> GenerateTextAsync([FromQuery] string prompt, [FromQuery] string model = "google/gemma-3-12b-it:free")
        {
            var requestBody = new
            {
                model,
                messages = new[] { new { role = "user", content = prompt } },
                temperature = 0.7,
                max_tokens = 1000,
                stream = true
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["DeepSeekKey"]);
            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://sso.d2s.com.vn");
            _httpClient.DefaultRequestHeaders.Add("X-Title", "SMR");

            using var response = await _httpClient.PostAsJsonAsync("https://openrouter.ai/api/v1/chat/completions", requestBody);
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:"))
                    continue;

                var contentJson = line.Substring("data:".Length).Trim();
                if (contentJson == "[DONE]") yield break;

                using var jsonDoc = JsonDocument.Parse(contentJson);
                if (jsonDoc.RootElement.TryGetProperty("choices", out var choices) &&
                    choices[0].TryGetProperty("delta", out var delta) &&
                    delta.TryGetProperty("content", out var contentElement))
                {
                    var content = contentElement.GetString();
                    if (!string.IsNullOrEmpty(content))
                    {
                        yield return content;
                        await Task.Delay(50);
                    }
                }
            }
        }



    }

    public class ChatBotResponse
    {
        public string? Role { get; set; }
        public string? Content { get; set; }

    }

    public class HttpResponseMessageResult : IActionResult
    {
        private readonly HttpResponseMessage _httpResponseMessage;

        public HttpResponseMessageResult(HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            response.StatusCode = (int)_httpResponseMessage.StatusCode;

            foreach (var header in _httpResponseMessage.Headers)
            {
                response.Headers[header.Key] = string.Join(",", header.Value);
            }

            if (_httpResponseMessage.Content != null)
            {
                var contentHeaders = _httpResponseMessage.Content.Headers;
                foreach (var header in contentHeaders)
                {
                    response.Headers[header.Key] = string.Join(",", header.Value);
                }

                using var stream = await _httpResponseMessage.Content.ReadAsStreamAsync();
                await stream.CopyToAsync(response.Body);
            }
        }
    }
}
