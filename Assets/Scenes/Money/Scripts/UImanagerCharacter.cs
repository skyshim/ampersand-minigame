using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject CharacterPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCharacterPanel()
    {
        mainPanel.SetActive(false);
        CharacterPanel.SetActive(true);
    }

    public void ReturnToMainPanel()
    {
        mainPanel.SetActive(true);
        CharacterPanel.SetActive(false);
    }
}
