
using Mono.Data.Sqlite;
using Snake.Config;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.DataAccess
{
    public static class SQLResultParser
    {
        public static async Task<List<T>> Query<T>(string query)
        {
            // Classic method to get values from reader
            // using (var connection = new SqliteConnection(ConfigManager.Config.connectionString))
            // {
            //     connection.Open();
            //     // using(var command = connection.CreateCommand())
            //     // {
            //     //     var sqlQuery = @"SELECT * FROM [player]";
            //     //     command.CommandText = sqlQuery;
            //     //     var reader = command.ExecuteReader();
            //     //     while (reader.Read())
            //     //     {
            //     //         Debug.Log(reader["id"]);
            //     //     }
            //     // }
            //     connection.Close();
            // }

            using (var connection = new SqliteConnection(ConfigManager.Config.connectionString))
            {
                await connection.OpenAsync();
                var data = await connection.QueryAsync<T>(query);
                await connection.CloseAsync();
                return data.AsList<T>();
            }
        }


        public static async Task<T> QueryFirstOrDefault<T>(string query)
        {
            using (var connection = new SqliteConnection(ConfigManager.Config.connectionString))
            {
                await connection.OpenAsync();
                var data = await connection.QueryAsync<T>(query);
                var result = data.FirstOrDefault<T>();
                await connection.CloseAsync();
                return result;
            }
        }

        public static async Task Execute(string query)
        {
            using (var connection = new SqliteConnection(ConfigManager.Config.connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query);
                await connection.CloseAsync();
                return;
            }
        }
    }
}
