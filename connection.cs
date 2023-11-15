using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace telegramAPI
{
    public class connection
    {

        MySqlConnection conn;
        public connection()
        {
            conn = new MySqlConnection();
        }

        public bool DbConnection()
        {
            try
            {
                var v = "SERVER = 'server'; UID = 'uid'; PWD = 'pwd'; PORT = port";
                conn.ConnectionString = v;
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataTable DataSelect(string sql)
        {
            var dT = new DataTable();
            using (var cmd = new MySqlCommand() { Connection = conn, CommandText = sql })
            {
                using (var da = new MySqlDataAdapter() { SelectCommand = cmd })
                {
                    da.Fill(dT);
                }
            }
            return dT;
        }

        public async void DataExeCute(string sql)
        {
            var auto = 0;
            using (var cmd = new MySqlCommand() { Connection = conn, CommandText = sql })
            {
                await cmd.ExecuteNonQueryAsync();
                auto = (int)cmd.LastInsertedId;
            }
        }
    }
}
