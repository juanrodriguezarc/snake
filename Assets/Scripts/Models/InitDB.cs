using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class InitDB : MonoBehaviour
{
    // Start is called before the first frame update
    string conn;
        string DATABASE_NAME = "/snakedb.db";
    void Start()
    {
        string filepath = Application.dataPath + DATABASE_NAME;
        Debug.Log($"filepath={filepath}");
        conn = "URI=file:" + filepath;
        CreateATable();
    }
 
    private void CreateATable()
    {
        using (var connection = new SqliteConnection(conn))
        {
            connection.Open();
            using(var command = connection.CreateCommand())
            {
                var sqlQuery = @"CREATE TABLE IF NOT EXISTS [player] (
                        [id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
                        [name] VARCHAR(255)  NOT NULL,
                        [score] INTEGER DEFAULT '0' NOT NULL)";
                command.CommandText = sqlQuery;
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
