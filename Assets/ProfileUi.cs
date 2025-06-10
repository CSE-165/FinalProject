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

    private bool userIsDragging = false;
    private float lastRefreshTime = 0f;
    private float refreshInterval = 2f;
    void Start()
    {
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        if (!userIsDragging && Time.time - lastRefreshTime >= refreshInterval)
        {
            loadSliderInfo();
            lastRefreshTime = Time.time;
        }
    }

    public void OnSliderValueChanged(int index, float value)
    {
        if (index >= 0 && index < character.interests.Length)
        {
            character.interests[index].interestLevel = Mathf.RoundToInt(value * 100);
            interestLabels[index].text = character.interests[index].interestName + " " + character.interests[index].interestLevel;
        }
    }

    void loadSliderInfo()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            StartCoroutine(LerpSliderDown(i, character.interests[i].interestLevel / 100.0f, 1f));
            interestLabels[i].text = character.interests[i].interestName + " " + character.interests[i].interestLevel.ToString();
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        string likes = "Likes: " + string.Join(", ", character.likes);
        string dislikes = "Dislikes: " + string.Join(", ", character.dislikes);
        description.text = character.description + "\n" + likes + "\n" + dislikes;

        loadSliderInfo();

        // Add listeners for all sliders
        for (int i = 0; i < sliders.Length; i++)
        {
            int index = i; // Capture index for use in lambda
            sliders[i].onValueChanged.AddListener((value) => OnSliderValueChanged(index, value));
        }
    }
    

    private IEnumerator LerpSliderDown(int index, float targetPercent, float duration)
    {
        float startValue = sliders[index].value;
        float endValue = Mathf.Clamp01(targetPercent); // ensure value is between 0 and 1
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, endValue, elapsed / duration);
            sliders[index].value = newValue;

        

            yield return null;
        }

        // Ensure final value is exact
        sliders[index].value = endValue;
    }

    
    public void OnBeginDragSlider() => userIsDragging = true;
    public void OnEndDragSlider() => userIsDragging = false;
}
