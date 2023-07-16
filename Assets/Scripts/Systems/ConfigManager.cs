using System;
using UnityEngine;
using System.IO;

namespace snake.Config
{
    public class ConfigManager: Singleton<ConfigManager>
    {
        public static ConfigData Config { get; set; }

        public void SaveConfig()
        {
            //Convert the ConfigData object to a JSON string.
            string json = JsonUtility.ToJson(Config);

            //Write the JSON string to a file on disk.
            File.WriteAllText("config.json", json);
        }

        public void LoadConfig()
        {
            //Get the JSON string from the file on disk.
            string savedJson = File.ReadAllText("config.json");

            //Convert the JSON string back to a ConfigData object.
            Config = JsonUtility.FromJson<ConfigData>(savedJson);

            string filepath = Application.dataPath + Config.dbName;
            Debug.Log($"filepath={filepath}");

            Config.connectionString = "URI=file:" + filepath;
        }
    }


    [Serializable]
    public class ConfigData
    {
        public string language;
        public string dbName;
        public string connectionString;
    }
}
