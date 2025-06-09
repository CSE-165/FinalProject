using System.Collections;
using System.Text;
using UnityEngine;

// Define the classes needed to deserialize the JSON
// You can place these inside your TestGemini file or in a separate file.
[System.Serializable]
public class GeminiResponse
{
    public Candidate[] candidates;
}

[System.Serializable]
public class Candidate
{
    public Content content;
}

[System.Serializable]
public class Content
{
    public Part[] parts;
}

[System.Serializable]
public class Part
{
    public string text;
}


public class TestGemini : MonoBehaviour
{
    public GeminiAPI geminiAPI;

    private void Start()
    {
        //yield return new WaitForSeconds(0.5f); // Wait before sending the prompt

        StringBuilder prompt = new StringBuilder();

        prompt.AppendLine("You are simulating a group conversation among AI-driven NPCs in a virtual environment.");
        prompt.AppendLine("The topic of discussion is: \"AI\".\n");
        prompt.AppendLine("Here is detailed information about the NPCs involved in the conversation:\n");

        prompt.AppendLine("- npc_0");
        prompt.AppendLine("  Description: This is NPC 0");
        prompt.AppendLine("  Interest in topic: 80");
        prompt.AppendLine("  Likes: Technology, Coding");
        prompt.AppendLine("  Dislikes: Paperwork");
        prompt.AppendLine("  Relationships: npc_1 (+5)\n");

        prompt.AppendLine("- npc_1");
        prompt.AppendLine("  Description: This is NPC 1");
        prompt.AppendLine("  Interest in topic: 80");
        prompt.AppendLine("  Likes: Technology, Coding");
        prompt.AppendLine("  Dislikes: Paperwork");
        prompt.AppendLine("  Relationships: npc_2 (+5)\n");

        prompt.AppendLine("- npc_2");
        prompt.AppendLine("  Description: This is NPC 2");
        prompt.AppendLine("  Interest in topic: 80");
        prompt.AppendLine("  Likes: Technology, Coding");
        prompt.AppendLine("  Dislikes: Paperwork");
        prompt.AppendLine("  Relationships: npc_0 (+5)\n");

        prompt.AppendLine("Generate an engaging and natural conversation among the above NPCs based on their descriptions, likes, and dislikes.");
        prompt.AppendLine("Format the one turn of conversation strictly as:");
        prompt.AppendLine("npcID: message");
        prompt.AppendLine("Use only npcIDs from the list above.");
        prompt.AppendLine("Begin the conversation below with 6 total turns.");

        geminiAPI.SendPrompt(prompt.ToString(), (responseJson) =>
        {
            GeminiResponse parsedResponse = JsonUtility.FromJson<GeminiResponse>(responseJson);

            // Check for nulls and array lengths to avoid errors.
            if (parsedResponse != null && parsedResponse.candidates.Length > 0 &&
                parsedResponse.candidates[0].content != null &&
                parsedResponse.candidates[0].content.parts.Length > 0)
            {
                string aiMessage = parsedResponse.candidates[0].content.parts[0].text;

                Debug.Log("Parsed AI Message: " + aiMessage);
            }
            else
            {
                Debug.LogError("Could not parse Gemini response or the response was empty.");
            }
        });
    }
}