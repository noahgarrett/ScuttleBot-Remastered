using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RiotSharp;
using RiotSharp.Misc;
using ScuttleBot_Remastered.Resources;

namespace ScuttleBot_Remastered.Commands;

public class SummonerStats : BaseCommandModule
{
    private RiotApi api;
    
    public SummonerStats()
    {
        api = RiotApi.GetInstance(DotEnv.RiotKey, 500, 30000);
    }

    [Command("rank")]
    public async Task RankCommand(CommandContext ctx, [RemainingText] string name)
    {
        await ctx.TriggerTypingAsync();
        
        var summoner = await api.Summoner.GetSummonerByNameAsync(Region.Na, name);
        var rankedEntry = await api.League.GetLeagueEntriesBySummonerAsync(Region.Na, summoner.Id);
        var solo = rankedEntry.Single(x => x.QueueType == "RANKED_SOLO_5x5");

        var numOfGames = solo.Wins + solo.Losses;
        var winRate = ((float) solo.Wins / (float) numOfGames) * 100;

        var embed = new DiscordEmbedBuilder()
        {
            Title = $"{summoner.Name}'s Ranked Stats",
            Description = $"{solo.QueueType}",
            Color = DiscordColor.Blue,
            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail()
            {
                Height = 100,
                Url = solo.Tier switch
                {
                    "IRON" => Constants.IronUrl,
                    "BRONZE" => Constants.BronzeUrl,
                    "SILVER" => Constants.SilverUrl,
                    "GOLD" => Constants.GoldUrl,
                    "PLATINUM" => Constants.PlatinumUrl,
                    "MASTER" => Constants.MasterUrl,
                    "GRANDMASTER" => Constants.GrandmasterUrl,
                    "CHALLENGER" => Constants.ChallengerUrl,
                    _ => Constants.IronUrl
                },
                Width = 100
            }
        };

        embed.AddField("Rank", $"{solo.Tier} {solo.Rank}", true);
        embed.AddField("Stats", $"**Wins:** {solo.Wins}\n**Losses**: {solo.Losses}\n**Win Rate:** {winRate}%", true);

        await ctx.RespondAsync(embed);
    }
}