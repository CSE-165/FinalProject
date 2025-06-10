using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public Animator anim;
    public float position;
    public TMP_Text textBubble;

    [Header("Movement & Behavior Settings")]
    public float moveSpeed = 3f;
    [Tooltip("The NPC will look for a new group only when interest in the current topic drops below this value.")]
    public int interestThreshold = 10; // The new "boredom" threshold

    // --- State Machine ---
    private Vector3 destination;
    private NPCState state = NPCState.PARTICIPATING;
    public enum NPCState
    {
        PARTICIPATING,    // Checking for better groups
        MOVING            // Moving to a new group
    }

    public void Start()
    {
        LoadNPCData();
        StartCoroutine(InterestReducer());
        // The first Update() call will handle finding the initial group.
    }

    public void Update()
    {
        switch (state)
        {
            case NPCState.PARTICIPATING:
                anim.SetBool("walking", false);
                DecideOnBestTopic();
                //listen + speak animation
                break;

            case NPCState.MOVING:
                PerformMovement();
                //move animation (walk)
                anim.SetBool("walking", true);
                anim.SetBool("listening", false);
                anim.SetBool("talking", false);
                break;
        }
    }

    /// <summary>
    /// This is the "brain". It checks interests and decides if a move is needed.
    /// It runs every frame while in the PARTICIPATING state.
    /// </summary>
    void DecideOnBestTopic()
    {
        if (groups.groupData == null) return;

        // --- NEW LOGIC: The "Boredom Check" ---
        // If the character is already in a group...
        if (!string.IsNullOrEmpty(currTopic))
        {
            // ...check if they are still interested enough to stay.
            if (GetCurrentInterestLevel() >= interestThreshold)
            {
                // Interest is high enough. Do nothing. Stay put.
                anim.SetBool("listening", true);
                anim.SetBool("walking", false);
                anim.SetBool("talking", false);
                return;
            }
        }
        // --- END OF NEW LOGIC ---
        // If we reach this point, it means one of two things:
        // 1. The character has no group yet.
        // 2. The character IS in a group, but is bored (interest < threshold).
        // In either case, it's time to find the best possible group.

        int maxValue = 0;
        string bestTopic = ""; // Set to empty initially

        foreach (var group in groups.groupData)
        {
            foreach (var interest in interests)
            {
                if (interest.interestName == group.topic && interest.interestLevel > maxValue)
                {
                    maxValue = interest.interestLevel;
                    bestTopic = group.topic;
                }
            }
        }

        // If we found a valid topic AND it's different from our current one, let's move.
        // (The second condition is important if our interest dropped but no better group exists)
        if (!string.IsNullOrEmpty(bestTopic) && currTopic != bestTopic)
        {
            // If we are leaving a group, formally remove ourselves.
            if (!string.IsNullOrEmpty(currTopic))
            {
                groups.RemoveCharacterFromGroup(this, currTopic);
            }

            currTopic = bestTopic;
            groups.AddCharacterToGroup(this, bestTopic);
            Vector3 groupPosition = groups.GetGroupPosition(bestTopic);

            position = Random.Range(0f, 36f);
            float angle = position * 10f;
            float angleRad = angle * Mathf.Deg2Rad;

<<<<<<< Updated upstream
            Vector3 pos = new Vector3(Mathf.Cos(angleRad) * 3f, 0f, Mathf.Sin(angleRad) * 3f);
            groupPosition.y = 0f;
=======
            Vector3 pos = new Vector3(Mathf.Cos(angleRad) * 8f, 0f, Mathf.Sin(angleRad) * 8f);

>>>>>>> Stashed changes
            destination = groupPosition + pos;

            state = NPCState.MOVING;
        }
    }

    /// <summary>
    /// Gets the character's interest level in their current topic.
    /// </summary>
    /// <returns>The interest level, or 0 if no topic is set.</returns>
    int GetCurrentInterestLevel()
    {
        if (string.IsNullOrEmpty(currTopic))
        {
            return 0;
        }
        foreach (var interest in interests)
        {
            if (interest.interestName == currTopic)
            {
                return interest.interestLevel;
            }
        }
        return 0; // Should not happen if data is consistent, but a safe fallback.
    }


    /// <summary>
    /// This is the "legs". It runs every frame ONLY when in the MOVING state.
    /// </summary>
    void PerformMovement()
    {
        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            state = NPCState.PARTICIPATING;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        Vector3 groupCenter = groups.GetGroupPosition(currTopic);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(groupCenter - transform.position), moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Loads NPC data from a JSON file located in the Resources folder.
    /// </summary>
    public void LoadNPCData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("CharacterData/npcs");
        if (jsonFile == null) { Debug.LogError("npcs.json not found in Resources!"); return; }
        NPCDataList npcDataList = JsonUtility.FromJson<NPCDataList>(jsonFile.text);
        foreach (var data in npcDataList.npcList)
        {
            if (data.id == npcID)
            {
                description = data.description; dislikes = data.dislikes; likes = data.likes; interests = data.interests; relations = data.relations;
                return;
            }
        }
    }

    /// <summary>
    /// Broadcasts a message to the NPC's text bubble and increases interest in the current topic.
    /// </summary>
    /// <param name="message">The message to display in the text bubble.</param>
    public void Broadcast(string message)
    {
        textBubble.text = message;
        //set speaking flag true
        anim.SetBool("talking", true);
        anim.SetBool("listening", false);
        anim.SetBool("walking", false);
        AddInterest(20);
        Invoke("TalkingFalse", Random.Range(4f, 10f));
    }

    [System.Serializable] public class NPCDataList { public NPCData[] npcList; }
    [System.Serializable] public class NPCData { public string id; public string description; public string[] dislikes; public string[] likes; public Interests[] interests; public Relation[] relations; }
    [System.Serializable] public class Interests { public string interestName; public int interestLevel; }
    [System.Serializable] public class Relation { public string relationName; public int relationLevel; }

    private void AddInterest(int val)
    {
        foreach (var interest in interests)
            if (interest.interestName == currTopic && interest.interestLevel <= (100 - val))
                interest.interestLevel += val;
    }

    private void DecreaseInterest(int val)
    {
        foreach (var interest in interests)
            if (interest.interestName == currTopic && interest.interestLevel >= val)
                interest.interestLevel -= val;
    }

    IEnumerator InterestReducer()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(10f);
            DecreaseInterest(5);
        }
    }

    private void TalkingFalse()
    {
        anim.SetBool("talking", false);
    }
}