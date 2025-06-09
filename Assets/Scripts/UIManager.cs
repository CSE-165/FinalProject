using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject groupCanvas;
    public GameObject characterCanvas; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchToGroupMenu()
    {
        groupCanvas.SetActive(true);
        characterCanvas.SetActive(false);
    }

    public void SwitchToCharacterMenu()
    {
        characterCanvas.SetActive(true);
        groupCanvas.SetActive(false);
    }
}
