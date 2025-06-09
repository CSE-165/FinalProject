using System.Collections;
using UnityEngine;

public class PromptManagerTester : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public PromptManager promptManager;
    public GeminiAPI geminiAPI;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("PromptManagerTester started");
        // Setup mock group data
        var groupData = new Group.GroupData
        {
            topic = "AI",
            characters = new System.Collections.Generic.List<Character>(),
            conversationHistory = new System.Collections.Generic.List<ConversationTurn>()
        };

        // Create dummy characters in memory (no need to be in scene)
        for (int i = 0; i < 3; i++)
        {
            Character dummy = new GameObject($"NPC_{i}").AddComponent<Character>();
            dummy.npcID = $"npc_{i}";
            dummy.description = $"This is NPC {i}";
            dummy.textBubble = new GameObject("Bubble").AddComponent<TMPro.TextMeshPro>(); // Prevent null

            dummy.interests = new Character.Interests[]
            {
                new Character.Interests { interestName = "AI", interestLevel = 80 }
            };

            dummy.likes = new string[] { "Technology", "Coding" };
            dummy.dislikes = new string[] { "Paperwork" };
            dummy.relations = new Character.Relation[]
            {
                new Character.Relation { relationName = $"npc_{(i + 1) % 3}", relationLevel = 5 }
            };

            groupData.characters.Add(dummy);
        }

        // Assign the GeminiAPI reference (you must assign this in the Unity inspector or via code)
        promptManager.geminiAPI = geminiAPI;

        // Run test conversation
        promptManager.GenerateMultiTurnConversation(groupData, 6);
    }
}
