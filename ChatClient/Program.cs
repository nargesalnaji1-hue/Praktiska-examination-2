﻿using ChatClient.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

string username = string.Empty;

while (string.IsNullOrWhiteSpace(username))
{
    Console.Write("Ange användarnamn: ");
    username = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrWhiteSpace(username))
    {
        Console.WriteLine("Användarnamnet får inte vara tomt. Försök igen.");
    }
}

var chatService = new ChatService(username);

try
{
    await chatService.StartAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Ett fel uppstod: {ex.Message}");
}
finally
{
    Console.WriteLine("Programmet avslutas. Tryck på valfri tangent...");
    Console.ReadKey();
}
