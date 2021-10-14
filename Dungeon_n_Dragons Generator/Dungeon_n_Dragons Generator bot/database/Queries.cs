using MySql.Data.MySqlClient;
using System;

namespace Dungeon_n_Dragons_Generator.Bot.Core.Database
{
    public class Queries
    {
        private readonly MySqlConnection _conn;

        public Queries(string database)
        {
            string connStr = $"server=127.0.0.1;user=root;database={database};port=3306;password=";
            _conn = new MySqlConnection(connStr);
        }

        public MySqlDataReader ExecuteReadQuery(string query)
        {
            var cmd = new MySqlCommand(query, _conn);
            var rdr = cmd.ExecuteReader();

            return rdr;
        }

        public bool ExecuteNonQuery(string query)
        {
            try
            {
                _conn.Open();
                {
                    MySqlCommand cmd = new MySqlCommand(query, _conn);
                    cmd.ExecuteNonQuery();
                }
                _conn.Close();
            }
            catch (Exception ex) { Console.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} Error\t{ex}"); return false; }

            return true;
        }

        public int ExecuteCountQuery(string query)
        {
            int count = 0;

            try
            {
                _conn.Open();
                {
                    MySqlCommand cmd = new MySqlCommand(query, _conn);
                    object result = cmd.ExecuteScalar();

                    count = Convert.ToInt32(result);
                }
                _conn.Close();
            }
            catch (Exception ex) { Console.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} Error\t{ex}"); return count; }

            return count;
        }

        public MySqlConnection Conn { get { return _conn; } }
    }
}