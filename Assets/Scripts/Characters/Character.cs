using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    
    // Start is called before the first frame update
    [Header("NPC Identifier")]
    public string npcID;

    [Header("Loaded NPC Data")]
    public string description;
    public Interests[] interests;
    public string[] dislikes;
    public string[] likes;
    public Relation[] relations;

    [Header("Group")]
    public Group groups;
    public string currTopic

    public void Start()
    {
        LoadNPCData();
    }

    public void LoadNPCData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Scripts/Characters/CharacterData/npcs");
        if (jsonFile == null)
        {
            Debug.LogError("npcs.json not found in Resources!");
            return;
        }

        NPCDataList npcDataList = JsonUtility.FromJson<NPCDataList>(jsonFile.text);
        foreach (var data in npcDataList.npcList)
        {
            if (data.id == npcID)
            {
                description = data.description;
                dislikes = data.dislikes;
                likes = data.likes;
                interests = data.interests;
                relations = data.relations;

                return;
            }
        }
    }

    private void Pathing()
    {
        int maxValue = 0;
        string topic = "";
        foreach (var interest in interests)
        {
            if (interest.interestLevel > maxValue)
            {
                maxValue = interest.interestLevel;
                topic = interest.interestName;
            }
        }

        if (currTopic != topic)
        {
            currTopic = topic;
            groups.AddCharacterToGroup(this, topic);
        }
    }

    public class NPCDataList
    {
        public NPCData[] npcList;
    }

    public class NPCData
    {
        public string id;
        public string description;
        public string[] dislikes;
        public string[] likes;
        public Interests[] interests;
        public Relation[] relations;
    }

    public class Interests
    {
        public string interestName;
        public int interestLevel;
    }

    public class Relation
    {
        public string relationName;
        public int relationLevel;
    }

}
