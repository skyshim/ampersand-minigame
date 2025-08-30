using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject levelUpPanel;
    public GameObject CharacterPanel;
    public GameObject SettingPanel;
    public GameObject ChallengePanel;
    public GameObject resetConfirmPanel;

    // Start is called before the first frame update
    void Start()
    {
        mainPanel.SetActive(true);
        levelUpPanel.SetActive(false);
        CharacterPanel.SetActive(false);
        SettingPanel.SetActive(false);
        ChallengePanel.SetActive(false);
        resetConfirmPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void OpenLevelUpPanel()
    {
        mainPanel.SetActive(false);
        ChallengePanel.SetActive(false);
        SettingPanel.SetActive(false);
        CharacterPanel.SetActive(false);
        levelUpPanel.SetActive(true);
        resetConfirmPanel.SetActive(false);
    }

    public void OpenCharacterPanel()
    {
        mainPanel.SetActive(false);
        ChallengePanel.SetActive(false);
        SettingPanel.SetActive(false);
        CharacterPanel.SetActive(true);
        levelUpPanel.SetActive(false);
        resetConfirmPanel.SetActive(false);
    }

    public void OpenSettingPanel()
    {
        mainPanel.SetActive(false);
        ChallengePanel.SetActive(false);
        SettingPanel.SetActive(true);
        CharacterPanel.SetActive(false);
        levelUpPanel.SetActive(false);
        resetConfirmPanel.SetActive(false);
    }
    public void OpenChallengePanel()
    {
        mainPanel.SetActive(false);
        ChallengePanel.SetActive(true);
        SettingPanel.SetActive(false);
        CharacterPanel.SetActive(false);
        levelUpPanel.SetActive(false);
        resetConfirmPanel.SetActive(false);
    }

    public void ReturnToMainPanel()
    {
        mainPanel.SetActive(true);
        ChallengePanel.SetActive(false);
        SettingPanel.SetActive(false);
        CharacterPanel.SetActive(false);
        levelUpPanel.SetActive(false);
        resetConfirmPanel.SetActive(false);
    }

    public void OpenResetConfiomPanel(){
        mainPanel.SetActive(false);
        ChallengePanel.SetActive(false);
        SettingPanel.SetActive(false);
        CharacterPanel.SetActive(false);
        levelUpPanel.SetActive(false);
        resetConfirmPanel.SetActive(true);
    }
}
