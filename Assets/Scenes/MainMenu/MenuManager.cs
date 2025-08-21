using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text popupTitleText;
    public TMP_Text popupDescriptionText;
    public TMP_Text rankingInfoText;
    public Button confirmButton;
    public Button xButton;

    private string nextSceneName;   // 확인 후 이동할 씬 이름

    private void Start()
    {
        popupPanel.SetActive(false);
        confirmButton.onClick.AddListener(OnConfirm);

        if (xButton != null)
            xButton.onClick.AddListener(ClosePopup);
    }
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }

    // 버튼에서 호출
    public void ShowPopup(string title, string description, string sceneName, bool hasRanking)
    {
        popupTitleText.text = title;
        popupDescriptionText.text = description;
        rankingInfoText.text = hasRanking ? "랭킹 지원" : "랭킹 없음";

        nextSceneName = sceneName;
        popupPanel.SetActive(true);
    }

    private void OnConfirm()
    {
        popupPanel.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}