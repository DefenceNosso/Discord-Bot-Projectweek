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
    class Class3
    {
        public class Town
        {         
            public string Dominant { get; set; }
            public string facilities { get; set; }
            public string name { get; set; }
            public string sizes { get; set; }
            public string surroundings { get; set; }
        }
        public class TownRoll : ModuleBase
        {
            private readonly Queries _queries;
            private readonly Random _rand;

            public TownRoll()
            {
                _queries = new Queries("towns");
                _rand = new Random();
            }

            [Command("rolltown")]
            [Alias("rt")]

            public async Task RollTown()
            {
                var msg = await ReplyAsync("***Generating Town***");
                var Town = new Town();
                string[] tables = { "dominant_race", "facilities", "name", "sizes", "surroundings" };
                string sql = "";
                for (int i = 0; i < tables.Length; i++)
                {
                    _queries.Conn.Open();
                    sql = $"SELECT * FROM {tables[i]} ORDER BY RAND() LIMIT 1";
                    var res0 = _queries.ExecuteReadQuery(sql);
                    while (res0.Read())
                     switch (i)
                    {
                        case 0:
                            Town.Dominant = res0.GetString(1);
                            break;
                        case 1:
                            Town.facilities = res0.GetString(1);
                            break;
                        case 2:
                            Town.name = res0.GetString(1);
                            break;
                        case 3:
                            Town.sizes = res0.GetString(1);
                            break;
                        case 4:
                            Town.surroundings = res0.GetString(1);
                            break;
                    }
                    _queries.Conn.Close();
                 }
                await msg.DeleteAsync();
                await ReplyAsync($"{Town.name}\n" + $"{Town.Dominant}\n" + $"{Town.facilities}\n" + $"{Town.sizes}\n" + $"{Town.surroundings}");
            }


        }
    }
}
