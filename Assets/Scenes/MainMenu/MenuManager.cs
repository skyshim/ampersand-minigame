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

    private string nextSceneName;   // Ȯ�� �� �̵��� �� �̸�

    private void Start()
    {
        popupPanel.SetActive(false);
        confirmButton.onClick.AddListener(OnConfirm);
    }

    // ��ư���� ȣ��
    public void ShowPopup(string title, string description, string sceneName, bool hasRanking)
    {
        popupTitleText.text = title;
        popupDescriptionText.text = description;
        rankingInfoText.text = hasRanking ? "��ŷ ����" : "��ŷ ����";

        nextSceneName = sceneName;
        popupPanel.SetActive(true);
    }

    private void OnConfirm()
    {
        popupPanel.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}