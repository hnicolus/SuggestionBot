using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuggestionBot;
using SuggestionBot.Modules.Suggestions;

var discordConfig = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build()
    .GetSection("Discord")
    .Get<DiscordConfiguration>();

async Task RunBotAsync()
{

    DiscordSocketClient client = new DiscordSocketClient();
    CommandService commands = new CommandService();

    IServiceProvider services = new ServiceCollection()
    .AddTransient<SuggestionService>()
    .AddTransient<HttpClient>()
    .AddSingleton(client)
    .AddSingleton(commands)
    .BuildServiceProvider();

    client.Log += async (LogMessage args) =>
    {
        Console.WriteLine(args);
        await Task.CompletedTask;
    };

    client.MessageReceived += async (SocketMessage arg) =>
    {
        var message = arg as SocketUserMessage;
        if (message is null || message.Author.IsBot) return;

        var context = new SocketCommandContext(client, message);

        int argPos = 0;
        if (message.HasStringPrefix("!", ref argPos))
        {
            var reply = await commands.ExecuteAsync(context, argPos, services);
            if (!reply.IsSuccess) Console.WriteLine(reply.Error);

            if (reply.Error.Equals(CommandError.UnmetPrecondition))
                await message.Channel.SendMessageAsync(reply.ErrorReason);
        }
    };

    await client.LoginAsync(TokenType.Bot, discordConfig.Token);
    await client.StartAsync();
    await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
    await Task.Delay(-1);
}

await Task.Run(() => RunBotAsync());