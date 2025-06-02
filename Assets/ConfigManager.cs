using UnityEngine;
using System.IO;

[System.Serializable]
public class ConfigData
{
    public string apiKey;
}

public class ConfigManager : MonoBehaviour
{
    public static ConfigData Config;

    private void Awake()
    {
        // Construct the full path to the file in Assets
        string filePath = Path.Combine(Application.dataPath, "config.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Config = JsonUtility.FromJson<ConfigData>(json);
            
        }
        else
        {
            Debug.LogError("Config file not found at: " + filePath);
        }
    }
}
