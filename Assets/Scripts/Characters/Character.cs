using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Character class represents a non-player character (NPC) in the game.
/// It contains properties for NPC identification, loaded data, group affiliation,
/// and player utilities.
/// </summary>
public class Character : MonoBehaviour
{


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
    public string currTopic;

    [Header("Player Utilities")]
    public Animation playerAnimation;
    public float position;
    public TMP_Text textBubble;
    private bool active = false; // Indicates if the character is active in the game

    private Vector3 destination; //If NPC decides to move, destination.
    private float thinkingTimer, timeToThink, moveSpeed, interestThreshold, npcInterestLevel = 10;
    public enum NPCState
    {
        PARTICIPATING,    // In a group and content for now.
        CONSIDERING_MOVE, // Interest has dropped; thinking about leaving for a few seconds.
        AWAITING_EXIT,    // Decided to leave, waiting for a turn to speak.
        MOVING,           // Actively walking to a new group.
        WANDERING         // No groups are interesting; taking a break.
    }

    private NPCState state = NPCState.PARTICIPATING;
    public void Start()
    {
        LoadNPCData();
        Broadcast("hi my name sophie");
    }

    public void Update()
    {
        if (!active)
        {
            return;
        }
        //Pathing();

        switch (state)
        {
            case NPCState.PARTICIPATING:
                
                break;

            case NPCState.MOVING:

                break;

            case NPCState.CONSIDERING_MOVE:

                break;

            case NPCState.AWAITING_EXIT:

                break;

            case NPCState.WANDERING:

                break;                
        }
    }

    /// <summary>
    /// Loads NPC data from a JSON file located in the Resources folder.
    /// </summary>
    public void LoadNPCData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("CharacterData/npcs");
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


    /// <summary>
    /// Activates the character, allowing it to start pathing and interacting with groups.
    /// </summary>
    private void Pathing()
    {
        int maxValue = 0;
        string topic = "";

        // Checks for current max interest level.
        foreach (var interest in interests)
        {
            if (interest.interestLevel > maxValue)
            {
                maxValue = interest.interestLevel;
                topic = interest.interestName;
            }
        }
        // If new max interest topic is found, update the current topic and move character.
        if (currTopic != topic)
        {
            currTopic = topic;
            groups.AddCharacterToGroup(this, topic);

            Vector3 groupPosition = groups.GetGroupPosition(topic);

            position = Random.Range(0f, 36f);

            while (!groups.CheckCharacterPosition(currTopic, position))
            {
                position = Random.Range(0f, 36f);
            }

            float angle = position * 10f;
            float angleRad = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(
                Mathf.Cos(angleRad) * 8f,
                0f, 
                Mathf.Sin(angleRad) * 8f
            );

            transform.position = Vector3.Lerp(transform.position, groupPosition + pos, Time.deltaTime * 0.5f);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(groupPosition), Time.deltaTime * 0.5f);
            //playerAnimation.Play("Walk");
        }
    }


    /// <summary>
    /// Broadcasts a message to other characters in the group.
    /// </summary>
    public void Broadcast(string message)
    {
        Debug.Log($"{npcID} broadcasts: {message}");
        // Here you can implement the logic to send the message to other characters in the group
        textBubble.text = message;
    }

    /// <summary>
    /// Data structure for holding a list of NPC data.
    /// </summary>
    public class NPCDataList
    {
        public NPCData[] npcList;
    }

    /// <summary>
    /// Data structure for holding individual NPC data.
    /// </summary>
    public class NPCData
    {
        public string id;
        public string description;
        public string[] dislikes;
        public string[] likes;
        public Interests[] interests;
        public Relation[] relations;
    }

    /// <summary>
    /// Data structure for holding interests and their levels.
    /// </summary>
    public class Interests
    {
        public string interestName;
        public int interestLevel;
    }

    /// <summary>
    /// Data structure for holding relations and their levels.
    /// </summary>
    public class Relation
    {
        public string relationName;
        public int relationLevel;
    }

}
