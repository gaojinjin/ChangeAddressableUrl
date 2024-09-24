using UnityEngine;
using System.IO;
namespace NewLoader
{
    public class ServerConfigLoader
    {
        public static string LoadServerUrl()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "ServerConfig.json");
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                Debug.Log(jsonContent);
                ServerConfig config = JsonUtility.FromJson<ServerConfig>(jsonContent);
                return config.ServerUrl;
            }
            return "http://127.0.0.1"; // Ä¬ÈÏµØÖ·
        }
    }
}