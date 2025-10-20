using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopbarManager : MonoBehaviour
{
    public void OnCreditClicked()
    {
        Debug.Log("크레딧 버튼 클릭");
        SceneManager.LoadScene("Credit");
    }

    public void OnExitClicked()
    {
        Debug.Log("나가기 버튼 클릭");
        Application.Quit(); // 빌드된 게임에서 종료
        // 에디터에서는 작동 안 함
    }
}
