using SocketIOClient;
using ChatClient.Models;

namespace ChatClient.Services;

public class ChatService
{
    private readonly string _username;
    private readonly SocketIO _client;
    private bool _isRunning;

    public ChatService(string username)
    {
        _username = username;

        _client = new SocketIO("wss://api.leetcode.se", new SocketIOOptions
        {
            Path = "/sys25d",
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        RegisterEvents();
    }

    private void RegisterEvents()
    {
        _client.OnConnected += async (_, _) =>
        {
            Console.WriteLine(new SystemEventMessage("Du är ansluten."));
            await _client.EmitAsync("join", new object[] { _username });
        };

        _client.OnDisconnected += (_, _) =>
        {
            Console.WriteLine(new SystemEventMessage("Du kopplades från servern."));
        };

        _client.On("message", async ctx =>
        {
            var sender = ctx.GetValue<string>(0) ?? "Okänd";
            var text = ctx.GetValue<string>(1) ?? "";

            Console.WriteLine(new ChatMessage(sender, text));
            await Task.CompletedTask;
        });

        _client.On("system", async ctx =>
        {
            var text = ctx.GetValue<string>(0) ?? "Systemmeddelande";
            Console.WriteLine(new SystemEventMessage(text));
            await Task.CompletedTask;
        });
    }

    public async Task StartAsync()
    {
        _isRunning = true;

        Console.WriteLine("Ansluter...");
        await _client.ConnectAsync();

        await InputLoop();
        await StopAsync();
    }

    private async Task InputLoop()
    {
        Console.WriteLine("Skriv meddelanden. /quit för att avsluta.");

        while (_isRunning)
        {
            var input = Console.ReadLine();

            if (input == null)
                continue;

            if (input.Trim().Equals("/quit", StringComparison.OrdinalIgnoreCase))
            {
                _isRunning = false;
                break;
            }

            if (string.IsNullOrWhiteSpace(input))
                continue;

            var msg = new ChatMessage(_username, input);

            await _client.EmitAsync("message", new object[]
            {
                msg.Sender,
                msg.Text
            });

            Console.WriteLine(msg);
        }
    }

    public async Task StopAsync()
    {
        try
        {
            await _client.EmitAsync("leave", new object[] { _username });
        }
        catch {}

        if (_client.Connected)
            await _client.DisconnectAsync();

        _client.Dispose();
    }
}
