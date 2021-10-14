using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Discord;
using Discord.Commands;
using Dungeon_n_Dragons_Generator.Bot.Core.Database;
using MySql.Data.MySqlClient;

namespace Dungeon_n_Dragons_Generator_bot.Commands
{
    public class Character
    {
        public Character()
        {
            var rand = new Random();

            if (rand.Next(2) == 1) Gender = "Male";
            else Gender = "Female";
        }

        public string Gender { get; }
        public string Race { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Background { get; set; }
        public string Class { get; set; }
        public string Fear { get; set; }
        public string Wants { get; set; }
    }
    public class NPCRoll : ModuleBase
    {
        private readonly Queries _queries;
        private readonly Random _rand;

        public NPCRoll() {
            _queries = new Queries("characters");
            _rand = new Random();
        }

   

        [Command("rollnpc")]
        [Alias("rn")]

        public async Task RollNpc()
        {
            var msg = await ReplyAsync("***Generating Npc***");

            var character = new Character();

            string sql = "";
            string[] tables = { "races", "name", "last_name", "backgrounds", "classes", " fears", "wants" };
            _queries.Conn.Open();
            {
                for (int i = 0; i < tables.Length; i++)
                {
                    sql = $"SELECT * FROM {tables[i]} ORDER BY RAND() LIMIT 1";

                    switch (i)
                    {
                        case 0:
                            var res0 = _queries.ExecuteReadQuery(sql);
                            while (res0.Read()) character.Race = res0.GetString(1);
                            res0.Close();
                            break;
                        case 1:
                            switch (character.Race)
                            {
                                case "Half-Elf": sql = $"SELECT * FROM {tables[i]}  WHERE (Race = 'Elf' OR Race = 'Human') AND Gender = '{character.Gender}' ORDER BY RAND() LIMIT 1"; break;
                                case "Half-Orc": sql = $"SELECT * FROM {tables[i]}  WHERE (Race = 'Orc' OR Race = 'Human') AND Gender = '{character.Gender}' ORDER BY RAND() LIMIT 1"; break;
                                default: sql = $"SELECT * FROM {tables[i]}  WHERE Race = '{character.Race}' AND Gender = '{character.Gender}' ORDER BY RAND() LIMIT 1"; break;
                            }

                            var res1 = _queries.ExecuteReadQuery(sql);
                            while (res1.Read()) character.Name = res1.GetString(1);
                            res1.Close();
                            break;
                        case 2:
                            switch (character.Race)
                            {
                                case "Half-Elf": sql = $"SELECT * FROM {tables[i]}  WHERE (Race = 'Elf' OR Race = 'Human') ORDER BY RAND() LIMIT 1"; break;
                                case "Half-Orc": sql = $"SELECT * FROM {tables[i]}  WHERE (Race = 'Orc' OR Race = 'Human') ORDER BY RAND() LIMIT 1"; break;
                                case "Tiefling": sql = $"SELECT * FROM {tables[i]}  WHERE Race = 'Human' ORDER BY RAND() LIMIT 1"; break;
                                default: sql = $"SELECT * FROM {tables[i]} WHERE Race = '{character.Race}' ORDER BY RAND() LIMIT 1"; break;
                            }

                            var res2 = _queries.ExecuteReadQuery(sql);
                            while (res2.Read()) character.LastName = res2.GetString(1);
                            res2.Close();
                            break;
                        case 3:
                            var res3 = _queries.ExecuteReadQuery(sql);
                            while (res3.Read()) character.Background = res3.GetString(1);
                            res3.Close();
                            break;
                        case 4:
                            var res4 = _queries.ExecuteReadQuery(sql);
                            while (res4.Read()) character.Class = res4.GetString(1);
                            res4.Close();
                            break;
                        case 5:
                            var res5 = _queries.ExecuteReadQuery(sql);
                            while (res5.Read()) character.Fear = res5.GetString(1);
                            res5.Close();
                            break;
                        case 6:
                            var res6 = _queries.ExecuteReadQuery(sql);
                            while (res6.Read()) character.Wants = res6.GetString(1);
                            res6.Close();
                            break;
                    }
                }
            }
            _queries.Conn.Close();

            await Task.Delay(1000);

            await msg.DeleteAsync();
            await ReplyAsync($"{character.Race}\n{character.Gender}\n{character.Name}\n{character.LastName}\n{character.Background}\n{character.Class}\n{character.Fear}\n{character.Wants}");

        }

        [Command("rollnpc")]
        [Alias("rn")]
        public async Task InvalidRollNpcAsync([Remainder] string wrong)
        {
            string cmd = "rollNpc";
            string description = "it will generate a random npc and display the following in info : Name, Last Name, Race, class, background, what the npc wants, what the npc fears";
            string usage = "d.rollnpc";
            string[] aliases = { "rn"};
            string[] param = { "None" };

            await ReplyAsync(MsgBuilder(cmd, description, usage, aliases, param));
        }

        private string MsgBuilder(string cmd, string description, string usage, string[] aliases, string[] param)
        {
            //Arrays for the format append of the stringbuilder.
            string[] gotten = { cmd, description, usage, string.Join(", ", aliases), string.Join("\n\t", param) };
            string[] info = { "Command", "Description", "Aliases", "Usage", "Parameters" };

            var sb = new StringBuilder("**You used the RollNpc command  wrong. Proper usage(s):**");
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

