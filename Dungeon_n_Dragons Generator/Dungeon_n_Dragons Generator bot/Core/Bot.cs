using System;
using System.Threading;
using System.IO;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Interactive;

using Dungeon_n_Dragons_Generator.bot.Core.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Dungeon_n_Dragons_Genorator.Bot.Core
{
    /*
     * Private varibles start with _
     * Public varibles start with UPPERCASE
     * Local varibles start with lowercase
     */
    public class Bot
    {
        //Starts the bot
        public static void Main(string[] args)
            => new Bot().MainAsync().GetAwaiter().GetResult();


        private async Task MainAsync()
        {
            /*
             * Shards are used when the bot is in a massive amount of guilds
             * We use it now so that later we don't need to change it
             */
            var config = new DiscordSocketConfig
            {
                TotalShards = 1
            };


            using (var services = ConfigureServices(config))
            {
                //Gets the Discord client
                var client = services.GetRequiredService<DiscordShardedClient>();

                //Activates if the condition is met
                client.ShardReady += ReadyAsync;
                client.Log += LogAsync;

                //Initiallizes the command halndler service
                await services.GetRequiredService<CommandHandlerService>().InitiallizeAsync();

                var token = "ODg1ODU0MDQ1NTk4Nzc3NDA1.YTtF3g.fO5i9GuxViObw_p9rbU-Ih5FDhs";


                //Login the bot to the client and start the connection to it
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();

                //Makes it so that the bot doesn't stop after completing its start up
                await Task.Delay(Timeout.Infinite);
            }
        }

        //Sets up the config
        private ServiceProvider ConfigureServices(DiscordSocketConfig config)
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordShardedClient(config))
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<InteractiveService>()
                .BuildServiceProvider();
        }

        //Logs things to the console
        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        //Sets the bot's game status of the bot
        private async Task ReadyAsync(DiscordSocketClient shard)
        {
            var time = DateTime.Now;

            Console.WriteLine($"{time.Hour}:{time.Minute}:{time.Second} Info\tShard Number {shard.ShardId} is connected and ready!");

            await shard.SetGameAsync("d.help");
        }
    }
}