using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderstandButton : MonoBehaviour
{
    private StartSceneManager startScene;
    public GameObject warningPanel;
    // Start is called before the first frame update
    void Start()
    {
        startScene = FindObjectOfType<StartSceneManager>();
    }

    public void UnderStandClick()
    {
        startScene.doYouUnderstand = true;
        warningPanel.SetActive(false);
    }
}
