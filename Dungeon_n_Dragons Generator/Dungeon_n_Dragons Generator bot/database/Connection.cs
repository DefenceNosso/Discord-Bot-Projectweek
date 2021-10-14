using MySql.Data.MySqlClient;

using System.IO;

namespace Dungeon_n_Dragons_Generator.Core.Database
{
    public class Connection
    {
        public MySqlConnection Connect()
        {
            var ConnectionInfo = File.ReadAllLines(@"../../../Database/ConnectionInfo.txt");

            string connStr = $"server=127.0.0.1;user=root;database=characters;port=3306;password=";
            MySqlConnection conn = new MySqlConnection(connStr);

            return conn;
        }
    }
}
