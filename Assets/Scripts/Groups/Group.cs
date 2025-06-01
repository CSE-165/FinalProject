using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{

    [Header("Loaded Group Data")]
    public string[] topics;

    [Header("Group Position")]
    public Vector3[] groupPosition;

    [Header("NPC's In This Group")]
    public GroupData[] groupData;

    [Header("Utilities")]
    public GeminiAPI geminiAI;

    // Start is called before the first frame update
    public void Start()
    {
        LoadGroupData();

        if (groupPosition.Length != topics.Length)
        {
            Debug.LogError("Group positions and topics count mismatch!");
        }
    }

    public void LoadGroupData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Scripts/Groups/GroupData/topics");
        if (jsonFile == null)
        {
            Debug.LogError("topics.json not found in Resources!");
            return;
        }

        topics = JsonUtility.FromJson<string[]>(jsonFile.text);
    }

    private void SendPrompt(string topic, string prompt)
    {
        
    }

    public void AddCharacterToGroup(Character character, string topic)
    {
        foreach (var group in groupData)
        {
            if (group.topic == topic)
            {
                List<Character> characterList = new List<Character>(group.characters);
                characterList.Add(character);
                group.characters = characterList.ToArray();
                return;
            }
        }

        Debug.LogWarning($"Topic '{topic}' not found in group data.");
    }

    public void RemoveCharacterFromGroup(Character character, string topic)
    {
        foreach (var group in groupData)
        {
            if (group.topic == topic)
            {
                List<Character> characterList = new List<Character>(group.characters);
                characterList.Remove(character);
                group.characters = characterList.ToArray();
                return;
            }
        }

        Debug.LogWarning($"Topic '{topic}' not found in group data.");
    }

    public Vector3 GetGroupPosition(string topic)
    {
        return groupPosition[System.Array.IndexOf(topics, topic)];
    }
    

    [System.Serializable]
    public class GroupDataList
    {
        public GroupData[] groupList;
    }

    public class GroupData
    {
        public string topic;
        public Character[] characters;
    }
}