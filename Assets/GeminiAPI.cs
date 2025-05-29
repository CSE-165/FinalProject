using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class GeminiAPI : MonoBehaviour
{
    [Header("Replace this with your Gemini API Key")]
    private string apiKey;

    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=";

    private void Start()
    {
        apiKey = ConfigManager.Config.apiKey;
        
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key is missing! Please check your config.");
        }
    }

    public void SendPrompt(string prompt)
    {
        StartCoroutine(SendPromptCoroutine(prompt));
    }

    private IEnumerator SendPromptCoroutine(string prompt)
    {
        string fullUrl = apiUrl + apiKey;
        
        // Create the JSON body for the request
        string jsonBody = $@"
        {{
            ""contents"": [
                {{
                    ""parts"": [
                        {{
                            ""text"": ""{EscapeJsonString(prompt)}""
                        }}
                    ]
                }}
            ]
        }}";

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error + " | Response: " + request.downloadHandler.text);
            }
        }
    }

    private string EscapeJsonString(string input)
    {
        return input.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
    }


}
