using System;
using UnityEngine;
using System.IO;
using Snake.Models;
using Snake.Utility;

namespace Snake.Config
{
    public class ConfigManager: Singleton<ConfigManager>
    {
        public static ConfigData Config { get; set; }
        public static Player Current { get; set; }

            //Write the JSON string to a file on disk.
        public void SaveConfig()
        {
            string json = JsonUtility.ToJson(Config);
            File.WriteAllText("config.json", json);
        }

            //Get the JSON string from the file on disk.
        public void LoadConfig()
        {
            string savedJson = File.ReadAllText("config.json");
            Config = JsonUtility.FromJson<ConfigData>(savedJson);
            string filepath = Application.dataPath + Config.dbName;
            Debug.Log($"filepath={filepath}");
            Config.connectionString = "URI=file:" + filepath;
        }

        public void LoadCurrentUser(Player player)
        {
            Current = player;
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
