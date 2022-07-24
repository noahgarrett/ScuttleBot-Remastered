using DSharpPlus;
using DSharpPlus.CommandsNext;
using ScuttleBot_Remastered.Commands;

namespace ScuttleBot_Remastered;

public class Program
{
    private static void Main(string[] args)
    {
        MainAsync().GetAwaiter().GetResult();   
    }

    private static async Task MainAsync()
    {
        var discord = new DiscordClient(new DiscordConfiguration()
        {
            Token = DotEnv.BotToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All
        });
        var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
        {
            StringPrefixes = new[] {"!"}
        });

        commands.RegisterCommands<SummonerStats>();

        await discord.ConnectAsync();
        await Task.Delay(-1);
    }
}