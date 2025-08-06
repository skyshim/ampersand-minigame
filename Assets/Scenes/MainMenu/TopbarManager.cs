using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopbarManager : MonoBehaviour
{
    public void OnSettingsClicked()
    {
        // 설정 패널 열기 or 씬 전환
        Debug.Log("설정 버튼 클릭");
        // 예: UIManager.Instance.ShowSettings();
    }

    public void OnRankingClicked()
    {
        Debug.Log("랭킹 버튼 클릭");
        // 랭킹 화면 열기
        // 예: UIManager.Instance.ShowRanking();
    }

    public void OnExitClicked()
    {
        Debug.Log("나가기 버튼 클릭");
        Application.Quit(); // 빌드된 게임에서 종료
        // 에디터에서는 작동 안 함
    }
}
