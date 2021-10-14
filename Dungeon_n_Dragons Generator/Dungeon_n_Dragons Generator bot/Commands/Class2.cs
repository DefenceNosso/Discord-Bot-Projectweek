using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace Dungeon_n_Dragons_Generator.bot.Commands
{
    public class Invalid : ModuleBase
    {
        [Command("rollstats")]
        [Alias("rs", "rstats")]
        public async Task InvalidRollStatsAsync([Remainder] string wrong)
        {
            string cmd = "rollstats";
            string description = "It will roll 4d6 and removes the lowest number. This is done 6 times, 1 for every stat.";
            string usage = "d.rollstats";
            string[] aliases = { "rs", "rstats" };
            string[] param = { "None" };

            await ReplyAsync(MsgBuilder(cmd, description, usage, aliases, param));
        }

        [Command("singlerollstat")]
        [Alias("srs", "srstat")]
        public async Task SingleRollStatAsync()
        {
            string cmd = "singlerollstat";
            string description = "It will roll 4d6 and removes the lowest number.";
            string usage = "d.singlerollstat";
            string[] aliases = { "srs", "srstat" };
            string[] param = { "None" };

            await ReplyAsync(MsgBuilder(cmd, description, usage, aliases, param));
        }

        private string MsgBuilder(string cmd, string description, string usage, string[] aliases, string[] param)
        {
            //Arrays for the format append of the stringbuilder.
            string[] gotten = { cmd, description, usage, string.Join(", ", aliases), string.Join("\n\t", param) };
            string[] info = { "Command", "Description", "Aliases", "Usage", "Parameters" };

            var sb = new StringBuilder("**You rolled the command dice wrong. Proper usage(s):**");
            {
                sb.AppendLine();

                for (int i = 0; i <= info.GetUpperBound(0); i++)
                {
                    //The parameters are done in a different way than the others so it needs a specials format for it.
                    if (i == info.GetUpperBound(0)) sb.AppendFormat("**{0}:**\n\t{1}", info[i], gotten[i]);
                    else sb.AppendFormat("**{0}:** {1}", info[i], gotten[i]);

                    sb.AppendLine();
                }
            }
            return $"{sb}";
        }
    }
}