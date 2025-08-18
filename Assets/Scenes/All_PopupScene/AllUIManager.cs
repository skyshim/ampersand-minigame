using UnityEngine;
using UnityEngine.SceneManagement;

public class AllUIManager : MonoBehaviour
{
    public static AllUIManager Instance;

    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject rankingPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �Ŀ��� ����
        }
        else
        {
            Destroy(gameObject);
        }

        // ������ ���� �� �г� ��� ��Ȱ��ȭ
        if (loginPanel != null) loginPanel.SetActive(false);
        if (rankingPanel != null) rankingPanel.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }


    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        // �� ��ȯ �� ��� �˾� ����
        loginPanel.SetActive(false);
        rankingPanel.SetActive(false);
    }
    public void ShowLoginPanel()
    {
        if (loginPanel != null)
            loginPanel.SetActive(true);
    }

    public void HideLoginPanel()
    {
        if (loginPanel != null)
            loginPanel.SetActive(false);
    }

    public void ShowRankingPanel()
    {
        if (rankingPanel != null)
            rankingPanel.SetActive(true);
    }

    public void HideRankingPanel()
    {
        if (rankingPanel != null)
            rankingPanel.SetActive(false);
    }

    // ��� ��ɵ� �߰� ����
    public void ToggleLoginPanel()
    {
        if (loginPanel != null)
            loginPanel.SetActive(!loginPanel.activeSelf);
    }

    public void ToggleRankingPanel()
    {
        if (rankingPanel != null)
            rankingPanel.SetActive(!rankingPanel.activeSelf);
    }
}
