using System.Collections;
using UnityEngine;
public class TestGemini : MonoBehaviour
{
    public GeminiAPI geminiAPI;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);  // Wait half a second before sending the prompt

        geminiAPI.SendPrompt("Explain how AI works in a few words");
    }
}