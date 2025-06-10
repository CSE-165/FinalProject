using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public GameObject groupCanvas;
    public GameObject characterCanvas;

    public GameObject[] characterProfiles;

    public Group group;

    public List<TMP_Dropdown> dropdowns;

    private List<string> activeTopics;

    private List<string> nonActiveTopics;

    private string[] topics;

    // Start is called before the first frame update
    void Start()
    {
        activeTopics = new List<string>();
        nonActiveTopics = new List<string>();
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("GroupData/topics");
        if (jsonFile == null)
        {
            Debug.LogError("topics.json not found in Resources!");
            return;
        }

        topics = JsonUtility.FromJson<TopicData>(jsonFile.text).topics;

        foreach (var topic in topics)
        {
            if (activeTopics.Contains(topic))
            {
                continue;
            }
            else
            {
                nonActiveTopics.Add(topic);
            }
        }

        return;
    }

    public void SwitchToGroupMenu()
    {
        groupCanvas.SetActive(true);
        characterCanvas.SetActive(false);
        foreach (var profile in characterProfiles)
        {
            profile.SetActive(false);
        }
    }

    public void SwitchToCharacterMenu()
    {
        characterCanvas.SetActive(true);
        groupCanvas.SetActive(false);
        foreach (var profile in characterProfiles)
        {
            profile.SetActive(false);
        }
    }

    public void InitDropdowns()
    {
        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(new List<string>(nonActiveTopics));
        }

        for (int i = 0; i < 4; i++)
        {
            dropdowns[i].options.Insert(0, new TMP_Dropdown.OptionData(activeTopics[i])); // Show selected on top
            dropdowns[i].value = 0; // Force selection to top
            dropdowns[i].RefreshShownValue();
        }

    }

    void RefreshDropdowns()
    {
        activeTopics.Clear();

        foreach (var topic in group.GetGroupTopics())
        {
            activeTopics.Add(topic);
        }

        TextAsset jsonFile = Resources.Load<TextAsset>("GroupData/topics");
        if (jsonFile == null)
        {
            Debug.LogError("topics.json not found in Resources!");
            return;
        }

        topics = JsonUtility.FromJson<TopicData>(jsonFile.text).topics;

        nonActiveTopics.Clear();

        foreach (var topic in topics)
        {
            if (activeTopics.Contains(topic))
            {
                continue;
            }
            else
            {
                nonActiveTopics.Add(topic);
            }
        }

        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(new List<string>(nonActiveTopics));
        }

        for (int i = 0; i < 4; i++)
        {
            dropdowns[i].options.Insert(0, new TMP_Dropdown.OptionData(activeTopics[i])); // Show selected on top
            dropdowns[i].value = 0; // Force selection to top
            dropdowns[i].RefreshShownValue();
        }

    }

    void OnDropdownChanged1(int index)
    {
        string selected = dropdowns[0].options[index].text;
        group.ChangeGroupTopic(0, selected);
        RefreshDropdowns();
        dropdowns[0].options.Insert(0, new TMP_Dropdown.OptionData(selected)); // Show selected on top
        dropdowns[0].value = 0; // Force selection to top
        dropdowns[0].RefreshShownValue();
    }

    void OnDropdownChanged2(int index)
    {
        string selected = dropdowns[1].options[index].text;
        group.ChangeGroupTopic(1, selected);
        RefreshDropdowns();
        dropdowns[1].options.Insert(0, new TMP_Dropdown.OptionData(selected)); // Show selected on top
        dropdowns[1].value = 0; // Force selection to top
        dropdowns[1].RefreshShownValue();
    }

    void OnDropdownChanged3(int index)
    {
        string selected = dropdowns[2].options[index].text;
        group.ChangeGroupTopic(2, selected);
        RefreshDropdowns();
        dropdowns[2].options.Insert(0, new TMP_Dropdown.OptionData(selected)); // Show selected on top
        dropdowns[2].value = 0; // Force selection to top
        dropdowns[2].RefreshShownValue();
    }

    void OnDropdownChanged4(int index)
    {
        string selected = dropdowns[3].options[index].text;
        group.ChangeGroupTopic(3, selected);
        RefreshDropdowns();
        dropdowns[3].options.Insert(0, new TMP_Dropdown.OptionData(selected)); // Show selected on top
        dropdowns[3].value = 0; // Force selection to top
        dropdowns[3].RefreshShownValue();
    }

    public void SwitchSophieProfile()
    {
        groupCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        characterProfiles[0].SetActive(true);
    }

    public void SwitchRemyProfile()
    {
        groupCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        characterProfiles[1].SetActive(true);
    }

    public void SwitchLeonardProfile()
    {
        groupCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        characterProfiles[2].SetActive(true);
    }

    public void SwitchElizabethProfile()
    {
        groupCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        characterProfiles[3].SetActive(true);
    }

    public void SwitchDavidProfile()
    {
        groupCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        characterProfiles[4].SetActive(true);
    }

    public void SwitchSuzieProfile()
    {
        groupCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        characterProfiles[5].SetActive(true);
    }

    public void SwitchBryceProfile()
    {
        groupCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        characterProfiles[6].SetActive(true);
    }

    public void SwitchMeganProfile()
    {
        groupCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        characterProfiles[7].SetActive(true);
    }

    public class TopicData
    {
        public string[] topics;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        foreach (var topic in group.GetGroupTopics())
        {
            activeTopics.Add(topic);
        }

        LoadData();
        InitDropdowns();

        dropdowns[0].onValueChanged.AddListener(OnDropdownChanged1);
        dropdowns[1].onValueChanged.AddListener(OnDropdownChanged2);
        dropdowns[2].onValueChanged.AddListener(OnDropdownChanged3);
        dropdowns[3].onValueChanged.AddListener(OnDropdownChanged4);
    }



}