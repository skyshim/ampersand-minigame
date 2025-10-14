using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BM : MonoBehaviour
{
    private UM um;
    private GameResult gr;
    private GM gm;
    [SerializeField] private GameObject ItemOn;
    [SerializeField] private GameObject ItemOff;

    void Start() {
        um = FindObjectOfType<UM>();
        gr = FindObjectOfType<GameResult>();
        gm = FindObjectOfType<GM>();
    }


    public void ClickPlayBtn() {
        um.CloseUI();
    }

    public void ClickMenuBtn() {
        um.OpenUI(4);
    }
    
    public void ClickContinueBtn() {
        um.CloseUI();
        if (gr.isGameClear) {
            gr.isGameOver = false;
        }
    }

    public void ClickResetBtn() {
        gr.ResetGame();
        StartCoroutine(StopRunning(0.01f)); // 0.5초 동안 동작 중단
        um.CloseUI();
    }

    public void ClickExitBtn() {
        SceneManager.LoadScene("MainMenu");
    }

    //public void ClickItemBtn() {
    //    if (gm.isItemMode) {
    //        gm.isItemMode = false;
    //        ItemOff.SetActive(true);
    //        ItemOn.SetActive(false);
    //    }
    //    else {
    //        gm.isItemMode = true;
    //        ItemOff.SetActive(false);
    //        ItemOn.SetActive(true);
    //    }
    //}

    IEnumerator StopRunning(float seconds) {
        // 원하는 동작 중단
        enabled = false;  // 이 스크립트 자체를 끔
        yield return new WaitForSeconds(seconds);
        enabled = true;   // 다시 켬
    }
}
