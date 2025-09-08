using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using platychat_dotnet.Models;
using platychat_dotnet.Repositories;
using platychat_dotnet.Services;

namespace platychat_dotnet.Hubs;

public class ChatHub : Hub
{
    private readonly IChatRepository _repo;
    private readonly IChatAiService _aiService;
    private static readonly ConcurrentDictionary<string, string> ConnectionIdToUser = new();

    public ChatHub(IChatRepository repo, IChatAiService aiService)
    {
        _repo = repo;
        _aiService = aiService;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"].FirstOrDefault();
        Console.WriteLine($"[ChatHub] Connection established: {Context.ConnectionId}, userId={userId}");

        if (!string.IsNullOrEmpty(userId))
        {
            ConnectionIdToUser[Context.ConnectionId] = userId;
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
        }

        await base.OnConnectedAsync(); // ✅ correctly awaited
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        if (ConnectionIdToUser.TryRemove(Context.ConnectionId, out var userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
            Console.WriteLine($"[ChatHub] Disconnected: {Context.ConnectionId}, userId={userId}");
        }
        if (ex != null)
        {
            Console.WriteLine($"[ChatHub] Disconnect error: {ex.Message}");
        }
        await base.OnDisconnectedAsync(ex);
    }

    public async Task SendUserMessage(string conversationId, string userId, string text)
    {
        try
        {
            // 1. Save user message (not sending it back to the same client)
            var msg = new Chat
            {
                ConversationId = conversationId,
                SenderId = userId,
                SenderType = "user",
                Text = text,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.SaveMessageAsync(msg);

            // 2. Bot typing indicator (true = typing)
            await Clients.Group($"user:{userId}").SendAsync("BotProcessing", conversationId, true);

            // 3. Get AI reply
            string botText;
            try
            {
                botText = await _aiService.GetReplyAsync(conversationId, userId, text);
            }
            catch (Exception aiEx)
            {
                Console.WriteLine($"[ChatHub] AI error: {aiEx.Message}");
                botText = "⚠️ Sorry, I had trouble generating a reply.";
            }

            // 4. Save bot message
            var botMsg = new Chat
            {
                ConversationId = conversationId,
                SenderId = "bot",
                SenderType = "bot",
                Text = botText,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.SaveMessageAsync(botMsg);

            // 5. Send bot reply to the client
            await Clients.Group($"user:{userId}").SendAsync("ReceiveMessage", botMsg);

            // 6. Bot typing indicator (false = done)
            await Clients.Group($"user:{userId}").SendAsync("BotProcessing", conversationId, false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatHub] SendUserMessage error: {ex.Message}");
            throw; // bubble up for SignalR error handling
        }
    }


    public Task Typing(string conversationId, string userId)
    {
        return Clients.Group($"user:{userId}").SendAsync("Typing", conversationId, userId);
    }
}
