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
            DontDestroyOnLoad(gameObject); // 씬 전환 후에도 유지
        }
        else
        {
            Destroy(gameObject);
        }

        // 시작할 때는 두 패널 모두 비활성화
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
        // 씬 전환 시 모든 팝업 끄기
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

    // 토글 기능도 추가 가능
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
