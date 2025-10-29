using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UM : MonoBehaviour
{
    private GameResult gr;

    [SerializeField] private GameObject bgf;
    [SerializeField] private GameObject window;

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject menuTitle;
    [SerializeField] private GameObject over;
    [SerializeField] private GameObject clear;
    [SerializeField] private GameObject menuBtn;
    [SerializeField] private GameObject resetBtn;
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private GameObject playBtn;
    [SerializeField] private GameObject exitBtn;
    [SerializeField] private UnityEngine.UI.Text FinalText;

    //심성우왔다감 :)
    public GameObject rankingPanel;
    public GameObject inputPanel;

    void Start() {
        gr = FindObjectOfType<GameResult>();
    }
    

    public void OpenUI(int code) {
        bgf.SetActive(true);
        window.SetActive(true);

        switch (code) {
            case 1: // 초기
                title.SetActive(true);
                playBtn.SetActive(true);
                //itemBtn.SetActive(true);
                break;
            case 2: // 게임오버
                over.SetActive(true);
                resetBtn.SetActive(true);
                //itemBtn.SetActive(true);
                rankingPanel.SetActive(true);
                FTOnOff(1);
                break;
            case 3: // 클리어
                clear.SetActive(true);
                continueBtn.SetActive(true);
                resetBtn.SetActive(true);
                FTOnOff(1);
                break;
            case 4: // 메뉴
                menuTitle.SetActive(true);
                continueBtn.SetActive(true);
                resetBtn.SetActive(true);
                exitBtn.SetActive(true);
                break;
        }

        gr.isGameOver = true;
    }


    public void CloseUI() {
        bgf.SetActive(false);
        window.SetActive(false);

        title.SetActive(false);
        menuTitle.SetActive(false);
        over.SetActive(false);
        clear.SetActive(false);
        resetBtn.SetActive(false);
        continueBtn.SetActive(false);
        playBtn.SetActive(false);
        exitBtn.SetActive(false);
        FTOnOff(0);

        rankingPanel.SetActive(false);
        inputPanel.SetActive(false);

        gr.isGameOver = false;
    }


    public void FTOnOff(int tf) {
        if (tf == 1) {
            Color color = FinalText.color;
            color.a = 1f;
            FinalText.color = color;
        }
        else if (tf == 0) {
            Color color = FinalText.color;
            color.a = 0f;
            FinalText.color = color;
        }
    }

}