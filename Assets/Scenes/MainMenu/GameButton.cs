using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    public string title;             // �˾��� ǥ���� ����
    [TextArea]
    public string description;       // �˾� ����
    public string sceneName;         // ��ȯ�� �� �̸�
    public bool hasRanking;          // ��ŷ ����

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
        // �޴� �Ŵ����� ���� ����
        menuManager.ShowPopup(title, description, sceneName, hasRanking);
    }
}