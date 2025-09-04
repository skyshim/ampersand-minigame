using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BM : MonoBehaviour
{
    private UM um;
    private GameResult gr;

    void Start() {
        um = FindObjectOfType<UM>();
        gr = FindObjectOfType<GameResult>();
    }


    public void ClickPlayBtn() {
        um.CloseUI(1);
    }

    public void ClickMenuBtn() {
        um.OpenUI(4);
    }
    
    public void ClickContinueBtn() {
        um.CloseUI(4);
    }

    public void ClickResetBtn() {
        gr.ResetGame();
        StartCoroutine(StopRunning(0.01f)); // 0.5초 동안 동작 중단
        um.CloseUI(4);
    }

    public void ClickExitBtn() {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator StopRunning(float seconds) {
        // 원하는 동작 중단
        enabled = false;  // 이 스크립트 자체를 끔
        yield return new WaitForSeconds(seconds);
        enabled = true;   // 다시 켬
    }
}
