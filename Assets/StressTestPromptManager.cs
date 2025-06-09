using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PromptManagerStressTest : MonoBehaviour
{
    public PromptManager promptManager;
    public GeminiAPI geminiAPI;

    private void Start()
    {
        StartCoroutine(RunMultipleSimulations());
    }

    private IEnumerator RunMultipleSimulations()
    {
        // Simulate 2 different group topics
        List<string> topics = new List<string> { "AI", "Space Exploration" };

        for (int groupIndex = 0; groupIndex < topics.Count; groupIndex++)
        {
            Group.GroupData groupData = new Group.GroupData
            {
                topic = topics[groupIndex],
                characters = new List<Character>(),
                conversationHistory = new List<ConversationTurn>()
            };

            // Create 3 dummy NPCs for each topic
            for (int i = 0; i < 3; i++)
            {
                GameObject npcObj = new GameObject($"Group{groupIndex}_NPC{i}");
                Character npc = npcObj.AddComponent<Character>();
                npc.npcID = $"npc_{groupIndex}_{i}";
                npc.description = $"This is NPC {i} from group {groupIndex}";

                TextMeshPro tmp = new GameObject("Bubble").AddComponent<TextMeshPro>();
                tmp.fontSize = 1;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.text = "Hello!";
                tmp.enableAutoSizing = true;
                tmp.transform.SetParent(npc.transform);
                tmp.transform.localPosition = new Vector3(0, 2.0f, 0); // floating above the NPC
                npc.textBubble = tmp;

                // Interests for each group
                npc.interests = new Character.Interests[]
                {
                    new Character.Interests
                    {
                        interestName = topics[groupIndex],
                        interestLevel = Random.Range(60, 100)
                    }
                };

                npc.likes = new string[] { "Reading", "Science", $"Hobby_{i}" };
                npc.dislikes = new string[] { "Laziness", $"Task_{i}" };
                npc.relations = new Character.Relation[]
                {
                    new Character.Relation
                    {
                        relationName = $"npc_{groupIndex}_{(i + 1) % 3}",
                        relationLevel = Random.Range(-5, 10)
                    }
                };

                groupData.characters.Add(npc);
            }

            // Assign GeminiAPI to prompt manager (important!)
            promptManager.geminiAPI = geminiAPI;

            // Generate the conversation for this group
            Debug.Log($"\n==== Generating conversation for group {groupIndex} on topic '{topics[groupIndex]}' ====");
            promptManager.GenerateMultiTurnConversation(groupData, 6);

            // Wait a few seconds between requests to avoid flooding
            yield return new WaitForSecondsRealtime(5f);
            
        }
    }
}
