using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject popupPanel;
    public Text popupTitleText;
    public Image infoImage;
    public Image styleImage;
    public Text popupDescriptionText;
    public Button confirmButton;
    public Button xButton;

    private string nextSceneName;   // 확인 후 이동할 씬 이름

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
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
    public void ShowPopup(string title, string description, string sceneName, Sprite ingameImage, Sprite styleSprite)
    {
        popupTitleText.text = title;
        popupDescriptionText.text = description;
        infoImage.sprite = ingameImage;
        styleImage.sprite = styleSprite;

        nextSceneName = sceneName;
        popupPanel.SetActive(true);
    }

    private void OnConfirm()
    {
        popupPanel.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}