using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileUi : MonoBehaviour
{
    // Start is called before the first frame update
    public Character character;

    public TMP_Text description;

    public List<TMP_Text> interestLabels;
    public Slider[] sliders;


    void Start()
    {
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        loadSliderInfo();
    }

    void OnSliderValueChanged(float value)
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            character.interests[i].interestLevel = (int)(sliders[i].value * 100);
        }

        loadSliderInfo();
    }

    void loadSliderInfo()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = (character.interests[i].interestLevel)/100.0f;
            interestLabels[i].text = character.interests[i].interestName + " " + character.interests[i].interestLevel.ToString();
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        string likes = "Likes: ";
        foreach (var like in character.likes)
        {
            if (like == character.likes[character.likes.Length - 1])
            {
                likes += like;
            }
            else
            {
                likes += like + ", ";
            }

        }

        string dislikes = "Dislikes: ";
        foreach (var dislike in character.dislikes)
        {
            if (dislike == character.dislikes[character.dislikes.Length - 1])
            {
                dislikes += dislike;
            }
            else
            {
                dislikes += dislike + ", ";
            }

        }

        description.text = character.description + "\n" + likes + "\n" + dislikes;
        loadSliderInfo();
        
        foreach (var slider in sliders)
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

    }
}
