using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanagerSetting : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject SettingPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSettingPanel(){
        mainPanel.SetActive(false);
        SettingPanel.SetActive(true);
    }

    public void ReturnToMainPanel()
    {
        mainPanel.SetActive(true);
        SettingPanel.SetActive(false);
    }
}
