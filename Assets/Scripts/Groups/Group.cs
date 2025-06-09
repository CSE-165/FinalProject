using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Group class representes a collection of non-player characters (NPCs) that share a common topic.
/// </summary>
public class Group : MonoBehaviour
{

    [Header("Loaded Group Data")]
    public GroupData[] groupData;

    [Header("Group Position")]
    public GameObject[] groupPosition;

    [Header("Utilities")]
    public GeminiAPI geminiAI;

    public PromptManager promptManager;

    // Start is called before the first frame update
    public void Start()
    {
        groupData = new GroupData[4]; // Initialize with an empty array
        LoadGroupData();


        
        // if (groupPosition.Length != topics.Length)
        // {
        //     Debug.LogError("Group positions and topics count mismatch!");
        // }

    }

    /// <summary>
    /// Loads group data from a JSON file located in the Resources folder.
    /// </summary>
    public void LoadGroupData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("GroupData/topics");
        if (jsonFile == null)
        {
            Debug.LogError("topics.json not found in Resources!");
            return;
        }

        string[] topics = JsonUtility.FromJson<TopicData>(jsonFile.text).topics;

        for(int i = 0; i < 4; i++)
        {
            groupData[i] = new GroupData
            {
                topic = topics[i],
                characters = new List<Character>(),
                //conversationHistory = new List<ConversationTurn>()
            };
        }
        return;
    }

    public void ChangeGroupTopic(int index, string newTopic)
    {
        if (index < 0 || index >= groupData.Length)
        {
            Debug.LogError("Index out of bounds for group data.");
            return;
        }

        groupData[index].topic = newTopic;
        Debug.Log($"Group topic changed to {newTopic} at index {index}.");
    }

    /// <summary>
    /// Sends a prompt to the Gemini API for the specified topic.
    /// </summary>
    private void SendPrompt(string topic, string prompt)
    {

    }

    /// <summary>
    /// Adds a character to the group based on the specified topic.
    /// <param name="character">The character to add.</param>
    /// <param name="topic">The topic to which the character belongs.</param>
    /// </summary>
    public void AddCharacterToGroup(Character character, string topic)
    {
        foreach (var group in groupData)
        {
            if (group.topic == topic)
            {
                group.characters.Add(character);
                return;
            }
        }

        Debug.LogWarning($"Topic '{topic}' not found in group data.");
    }

    /// <summary>
    /// Removes a character from the group based on the specified topic.
    /// <param name="character">The character to remove.</param>
    /// <param name="topic">The topic from which the character should be removed.</param>
    /// </summary>
    public void RemoveCharacterFromGroup(Character character, string topic)
    {
        foreach (var group in groupData)
        {
            if (group.topic == topic)
            {
                group.characters.Remove(character);
                return;
            }
        }

        Debug.LogWarning($"Topic '{topic}' not found in group data.");
    }

    /// <summary>
    /// Gets the position of the group associated with the specified topic.
    /// </summary>
    public Vector3 GetGroupPosition(string topic)
    {
        for (int i = 0; i < groupData.Length; i++)
        {
            if (groupData[i].topic == topic)
            {
                if (i < groupPosition.Length)
                {
                    return groupPosition[i].transform.position;
                }
                else
                {
                    Debug.LogError($"Group position for topic '{topic}' not found.");
                    return Vector3.zero;
                }
            }
        }
        
        return Vector3.zero; // Return zero vector if topic not found
    }

    /// <summary>
    /// Sends a message to a specific NPC within the group based on the topic to broadcast.
    /// <param name="topic">The topic of the group.</param>
    /// <param name="message">The message to send.</param>
    /// <param name="npcID">The ID of the NPC to which the message should be sent.</param>
    /// </summary>
    public void SendMessageToNPC(string topic, string message, string npcID)
    {
        foreach (var group in groupData)
        {
            if (group.topic == topic)
            {
                foreach (var character in group.characters)
                {
                    if (character.npcID == npcID)
                    {
                        character.Broadcast(message);
                        return;
                    }
                }
            }
        }

    }

    /// <summary>
    /// Checks if a character with the specified position exists in the group for the given topic.
    /// </summary>
    public bool CheckCharacterPosition(string topic, float position)
    {
        foreach (var group in groupData)
        {
            if (group.topic == topic)
            {
                foreach (var character in group.characters)
                {
                    if (character.position == position)
                    {

                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Data structure for holding a list of group data.
    /// </summary>
    public class GroupDataList
    {
        public GroupData[] groupList;
    }

    /// <summary>
    /// Data structure for holding group data.
    /// </summary>
    public class GroupData
    {
        public string topic;
        public List<Character> characters;
        public List<ConversationTurn> conversationHistory = new List<ConversationTurn>();
    }

    public class TopicData
    {
        public string[] topics;
    }

}