using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class UM : MonoBehaviour
{
    private GameResult gr;

    [SerializeField] private GameObject bgf;
    [SerializeField] private GameObject window;

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject over;
    [SerializeField] private GameObject clear;
    [SerializeField] private GameObject menuBtn;
    [SerializeField] private GameObject resetBtn;
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private GameObject playBtn;
    [SerializeField] private GameObject itemBtn;
    [SerializeField] private GameObject exitBtn;

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
                break;
            case 3: // 클리어
                clear.SetActive(true);
                continueBtn.SetActive(true);
                resetBtn.SetActive(true);
                break;
            case 4: // 메뉴
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
        over.SetActive(false);
        clear.SetActive(false);
        resetBtn.SetActive(false);
        continueBtn.SetActive(false);
        playBtn.SetActive(false);
        //itemBtn.SetActive(false);
        exitBtn.SetActive(false);

        gr.isGameOver = false;
    }

    // 스크립트꺼서 잠시 대기
    IEnumerator StopRunning(float seconds) {
        // 원하는 동작 중단
        enabled = false;  // 이 스크립트 자체를 끔
        yield return new WaitForSeconds(seconds);
        enabled = true;   // 다시 켬
    }

}