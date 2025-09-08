using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace platychat_dotnet.Services;

public class ChatAiService : IChatAiService
{
    private readonly HttpClient _http;
    private readonly ILogger<ChatAiService> _log;
    private readonly string _apiKey;

    public ChatAiService(HttpClient http, IConfiguration config, ILogger<ChatAiService> log)
    {
        _http = http;
        _apiKey = config.GetSection("OpenAi").GetValue<string>("ApiKey") ?? throw new ArgumentNullException("OpenAI API key is not configured.");
        _log = log;
    }


    public async Task<string> GetReplyAsync(string conversationId, string userId, string userMessage)
    {
        var payload = new
        {
            model = "gpt-4o-mini",
            messages = new[] {
                new { role = "system", content = "You are a helpful chat assistant." },
                new { role = "user", content = userMessage }
            }
        };

        var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var res = await _http.SendAsync(req);
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadAsStringAsync();

        // parse result depends on provider; extract assistant content
        using var doc = JsonDocument.Parse(body);
        var assistant = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
        return assistant ?? "Sorry, I couldn't generate a reply.";
    }
}
