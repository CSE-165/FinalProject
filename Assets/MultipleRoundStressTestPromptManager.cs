using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PromptManagerMultiRoundStressTest : MonoBehaviour
{
    public PromptManager promptManager;
    public GeminiAPI geminiAPI;

    [Header("Test Settings")]
    public int numberOfGroups = 2;
    public int roundsPerGroup = 3;
    public int turnsPerRound = 6;
    public float delayBetweenRounds = 5f;

    private List<string> topics = new List<string> { "AI", "Climate Change", "Space Exploration", "Education" };

    private void Start()
    {
        StartCoroutine(RunStressTest());
    }

    private IEnumerator RunStressTest()
    {
        Dictionary<int, Group.GroupData> allGroups = new Dictionary<int, Group.GroupData>();

        for (int groupIndex = 0; groupIndex < numberOfGroups; groupIndex++)
        {
            string topic = topics[groupIndex % topics.Count];

            // Create group only once
            if (!allGroups.ContainsKey(groupIndex))
            {
                Group.GroupData groupData = new Group.GroupData
                {
                    topic = topic,
                    characters = new List<Character>(),
                    conversationHistory = new List<ConversationTurn>()
                };

                // Create dummy NPCs
                for (int i = 0; i < 4; i++)
                {
                    GameObject npcObj = new GameObject($"Group{groupIndex}_NPC{i}");
                    Character npc = npcObj.AddComponent<Character>();
                    npc.npcID = $"npc_{groupIndex}_{i}";
                    npc.description = $"NPC {i} discussing {topic}";
                    npc.textBubble = new GameObject("Bubble").AddComponent<TextMeshPro>();

                    npc.interests = new Character.Interests[]
                    {
                        new Character.Interests
                        {
                            interestName = topic,
                            interestLevel = Random.Range(60, 100)
                        }
                    };

                    npc.likes = new string[] { "Science", "Debate", $"Interest_{i}" };
                    npc.dislikes = new string[] { "Laziness", $"Dislike_{i}" };
                    npc.relations = new Character.Relation[]
                    {
                        new Character.Relation
                        {
                            relationName = $"npc_{groupIndex}_{(i + 1) % 4}",
                            relationLevel = Random.Range(-5, 10)
                        }
                    };

                    groupData.characters.Add(npc);
                }

                allGroups[groupIndex] = groupData;
            }

            Group.GroupData groupToUse = allGroups[groupIndex];

            // Run multiple conversation rounds
            for (int round = 0; round < roundsPerGroup; round++)
            {
                Debug.Log($"\n🧪 Group {groupIndex} | Topic: {groupToUse.topic} | Round {round + 1}/{roundsPerGroup}");
                promptManager.GenerateMultiTurnConversation(groupToUse, turnsPerRound);
                yield return new WaitForSecondsRealtime(delayBetweenRounds);
            }
        }

        Debug.Log("✅ Multi-round stress test complete.");
    }
}
