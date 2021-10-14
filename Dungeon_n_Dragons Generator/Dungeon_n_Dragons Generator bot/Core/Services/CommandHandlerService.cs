using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Dungeon_n_Dragons_Generator.bot.Core.Services
{
    public class CommandHandlerService
    {
        private readonly CommandService _commands;
        private readonly DiscordShardedClient _client;
        private readonly IServiceProvider _services;

        public CommandHandlerService(IServiceProvider services)
        {
            //Get the nessasary services
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordShardedClient>();
            _services = services;

            //If the command is executed this code acivates
            _commands.CommandExecuted += CommandExecutedAsync;
            //Logs what happens with the command
            _commands.Log += LogAsync;

            //Activates when the client detects a message
            _client.MessageReceived += MessageRecievedAsync;
        }

        //Initiallizes the Service.
        public async Task InitiallizeAsync()
            => await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        //Runs when the client has recieved a message
        private async Task MessageRecievedAsync(SocketMessage s)
        {
            //It checks if the message is an user message
            //If not then it will do nothing
            var msg = s as SocketUserMessage;
            if (msg == null) return;

            int argPos = 0;
            string[] prefix = { "d.", "D." };

            //Checks if the message has the prefix
            //If so then it will try to execute the command
            if (msg.HasStringPrefix(prefix[0], ref argPos) || msg.HasStringPrefix(prefix[1], ref argPos))
            {
                var context = new ShardedCommandContext(_client, msg);
                await _commands.ExecuteAsync(context, argPos, _services);
            }
        }


        //Checks if the execute was a success
        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (result.IsSuccess) return;

            //If the command does not exist it will type the following in hte chat
            if (!command.IsSpecified) { await context.Channel.SendMessageAsync("Command not found. use `d.help` to see a list of commands."); }
        }

        //writes the log to the console
        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}
