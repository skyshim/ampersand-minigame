using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subak_Ranking : MonoBehaviour
{
    public GameObject inputPanel; // 입력창 패널
    public InputField inputField; // InputField
    public Button submitButton;   // 제출 버튼


    public FirebaseRanking firebase;
    public GM gameManager;
    // Start is called before the first frame update

    private void Start()
    {
        firebase = FindAnyObjectByType<FirebaseRanking>();
        submitButton.onClick.AddListener(OnSubmit); 
    }

    public void OnMouseDown()
    {
        inputPanel.SetActive(true); // 버튼 누르면 입력창 활성화
        inputField.text = "";       // 초기화
    }

    private void OnSubmit()
    {
        Debug.Log("2");
        string nickname = inputField.text.Trim();
        if (!string.IsNullOrEmpty(nickname))
        {
            Debug.Log("닉네임 등록: " + nickname);
            inputPanel.SetActive(false); // 입력창 닫기

            // 여기서 랭킹 전송 함수 호출 가능
            firebase.UploadScore("수박게임", nickname, gameManager.score);
        }
        else
        {
            Debug.Log("닉네임을 입력하세요.");
        }
    }
}
