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
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject resetBtn;
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private GameObject playBtn;
    [SerializeField] private GameObject itemBtn;


    public bool isPaused = false;
    public int pauseCode = 0; // 0 - 없음 , 1 - 최초 시작 , 2 - 게임 오버 , 3 - 클리어 , 4 - 메뉴



    void Start() {
        gr = FindObjectOfType<GameResult>();
    }

    private void OnEnable() {
        bgf.SetActive(true);
        window.SetActive(true);

        switch (pauseCode) {
            case 1:
                title.SetActive(true);
                playBtn.SetActive(true);
                itemBtn.SetActive(true);
                break;
            case 2:
                over.SetActive(true);
                playBtn.SetActive(true);
                itemBtn.SetActive(true);
                break;
            case 3:     
                clear.SetActive(true);
                continueBtn.SetActive(true);
                resetBtn.SetActive(true);
                break;
            case 4:
                menu.SetActive(true);
                continueBtn.SetActive(true);
                resetBtn.SetActive(true);
                break;
        }
        isPaused = true;
    }


    void Update()
    {
        if (!isPaused) {
            StartCoroutine(Pause(0.01f)); // 0.5초 대기
            TurnOff();
        }
    }

    // 스크립트꺼서 잠시 대기
    IEnumerator Pause(float seconds) {
        // 원하는 동작 중단
        enabled = false;  // 이 스크립트 자체를 끔
        yield return new WaitForSeconds(seconds);
        enabled = true;   // 다시 켬
    }


    void TurnOff() {
        bgf.SetActive(false);
        window.SetActive(false);

        title.SetActive(false); 
        playBtn.SetActive(false);
        itemBtn.SetActive(false);

        gameObject.SetActive(false);
        gr.isGameOver = false;
    }
}
