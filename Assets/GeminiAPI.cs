using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class GeminiAPI : MonoBehaviour
{
    
    private string apiKey;  //Replace this with your Gemini API Key

    private string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=";

    private void Start()
    {
        apiKey = ConfigManager.Config.apiKey;

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key is missing! Please check your config.");
        }
    }

    /// <summary>
    /// Sends a prompt to Gemini and returns the LLM's response via callback.
    /// </summary>
    public void SendPrompt(string prompt, Action<string> onResponse)
    {
        StartCoroutine(SendPromptCoroutine(prompt, onResponse));
    }

    private IEnumerator SendPromptCoroutine(string prompt, Action<string> onResponse)
    {
        string fullUrl = apiUrl + apiKey;

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
                string rawJson = request.downloadHandler.text;

                // Extract response text
                string messageText = ExtractTextFromResponse(rawJson);
                onResponse?.Invoke(messageText);
            }
            else
            {
                Debug.LogError("Error: " + request.error + " | Response: " + request.downloadHandler.text);
                onResponse?.Invoke(null);
            }
        }
    }

    private string EscapeJsonString(string input)
    {
        return input.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r");
    }

    private string ExtractTextFromResponse(string json)
    {
        string x = "need to parse text{} in Gemini json response into string";
        return x;
    }
}
