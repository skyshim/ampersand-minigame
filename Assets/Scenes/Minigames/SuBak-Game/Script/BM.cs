using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BM : MonoBehaviour
{
    private UM um;
    private GameResult gr;
    private GM gm;

    void Start() {
        um = FindObjectOfType<UM>();
        gr = FindObjectOfType<GameResult>();
        gm = FindObjectOfType<GM>();

        firebase = FindAnyObjectByType<FirebaseRanking>();
        submitButton.onClick.AddListener(OnSubmit);
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

    IEnumerator StopRunning(float seconds) {
        // 원하는 동작 중단
        enabled = false;  // 이 스크립트 자체를 끔
        yield return new WaitForSeconds(seconds);
        enabled = true;   // 다시 켬
    }


    // 랭킹 설계
    public GameObject inputPanel; // 입력창 패널
    public InputField inputField; // InputField
    public Button submitButton;   // 제출 버튼
    public GameObject rankingButton; // 랭킹등록 버튼
    public Text rankingButtonText; // 랭킹등록 버튼 텍스트

    public FirebaseRanking firebase;

    public void ClickRankingBtn() {
        if (gm.isRankingSubmitted) {
            rankingButtonText.text = "이미 등록되었습니다.";
            Invoke("ChangeRankText", 1f);
            return;
        }
        inputPanel.SetActive(true); // 버튼 누르면 입력창 활성화
        inputField.text = "";       // 초기화
        rankingButton.SetActive(false); // 랭킹등록 버튼 비활성화
    }
    private void ChangeRankText() {
        rankingButtonText.text = "랭킹등록";
    }

    private void OnSubmit() {
        string nickname = inputField.text.Trim();
        if (!string.IsNullOrEmpty(nickname)) {
            Debug.Log("닉네임 등록: " + nickname);
            inputPanel.SetActive(false); // 입력창 닫기
            rankingButton.SetActive(true); // 랭킹등록 버튼 비활성화
            rankingButtonText.text = "등록완료"; // 버튼 텍스트 변경
            gm.isRankingSubmitted = true; // 중복 등록 방지 플래그 설정

            // 여기서 랭킹 전송 함수 호출 가능
            firebase.UploadScore("수박게임", nickname, gm.score);
        }
        else {
            Debug.Log("닉네임을 입력하세요.");
        }
    }
}
