using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;

using System;
using System.Threading.Tasks;

namespace Dungeon_n_Dragons_Generator.bot.Commands
{
    //You can put commands in as many classes as you want as long as the class inherits the Module or Interactive Base
    public class General : ModuleBase
    {
        //This is what you after the prefix to activate the code
        [Command("hello")]
        /*
         * The name of the async method doesn't matter as long as it if below [Command()]
         * It is typical for async methods to end their name with Async
         */
        public async Task HelloAsync()
        {
            var hello = new EmbedBuilder();
            {
                hello.WithTitle($"General Kenobi:");
                hello.WithDescription($"Hello there <@{Context.Message.Author.Id}>");
            }

            await ReplyAsync(embed: hello.Build());
        }

        [Command("ping")]
        public async Task PingAsync()
        {
            var msg = await ReplyAsync("***Calculating ping...***");

            await ReplyAsync($"Pong! ***{msg.Timestamp.Millisecond - DateTime.Now.Millisecond}***ms");

            await msg.DeleteAsync();
        }
    }

    public class GeneralI : InteractiveBase
    {
        [Command("delete")]
        public async Task<RuntimeResult> Test_DeleteAfterAsync()
        {
            await ReplyAndDeleteAsync("this message will delete in 10 seconds", timeout: TimeSpan.FromSeconds(10));
            return Ok();
        }

        [Command("testchart", RunMode = RunMode.Async)]
        public async Task HChartAsync()
        {
            bool active = true;
            string answer = "ru";

            await ReplyAsync("る");

            do
            {
                var response = await NextMessageAsync(timeout: TimeSpan.FromSeconds(5));

                if (response != null)
                {
                    if (response.Content.ToLower().Equals(answer))
                    {
                        await ReplyAsync("You guessed it");
                        active = false;
                    }
                    else { await ReplyAsync("I'm sorry but that is not the answer. Please try again."); }
                }
                else
                {
                    await ReplyAsync("You didn't reply in time ~desu");
                    active = false;
                }
            } while (active);
        }
       
    }
}
