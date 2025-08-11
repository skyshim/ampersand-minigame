using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    public string title;             // 팝업에 표시할 제목
    [TextArea]
    public string description;       // 팝업 본문
    public string sceneName;         // 전환할 씬 이름
    public bool hasRanking;          // 랭킹 유무

    private Button button;
    private MenuManager menuManager;

    private void Awake()
    {
        button = GetComponent<Button>();
        menuManager = FindObjectOfType<MenuManager>();

        if (button != null && menuManager != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        // 메뉴 매니저에 정보 전달
        menuManager.ShowPopup(title, description, sceneName, hasRanking);
    }
}