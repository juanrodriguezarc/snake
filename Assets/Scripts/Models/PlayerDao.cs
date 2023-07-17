
using System.Threading.Tasks;
using Snake.Models;
using Snake.Config;

namespace Snake.DataAccess
{
    public static class PlayerDao
    {
        public static async Task<Player> GetCurrentUser() => await SQLResultParser.QueryFirstOrDefault<Player>("SELECT * FROM [player]");

        public static async Task CreateTable()
        {
            const string query = @"CREATE TABLE IF NOT EXISTS [player] (
                        [id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
                        [name] VARCHAR(255)  NOT NULL,
                        [score] INTEGER DEFAULT '0' NOT NULL)";
            await SQLResultParser.Execute(query);
            return;
        }

        public static async Task<Player> UpdateCurrentScore(int score)
        {
            var id = ConfigManager.Current.id;
            string query = $"UPDATE [player] SET score = {score} WHERE {id}";
            return await SQLResultParser.QueryFirstOrDefault<Player>(query);
        }

    }
}