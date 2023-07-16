
using System.Threading.Tasks;
using snake.DataAccess;

public static class PlayerDao
{

    public static async Task<Player> GetCurrentUser()
    {
        Player data = await SQLResultParser.QueryFirstOrDefault<Player>("SELECT * FROM [player]");
        return data;

    }
    public static async Task InitDB()
    {
        var query = @"CREATE TABLE IF NOT EXISTS [player] (
                        [id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
                        [name] VARCHAR(255)  NOT NULL,
                        [score] INTEGER DEFAULT '0' NOT NULL)";
        await SQLResultParser.Execute(query);
        return;
    }
}
