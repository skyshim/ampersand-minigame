using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    private bool loginCompleted = false;
    private LoginManager loginManager;

    private void Awake()
    {
        // Additive 씬 로드, 완료 콜백 사용
        SceneManager.LoadSceneAsync("PopupScene", LoadSceneMode.Additive).completed += OnPopupSceneLoaded;
    }

    private void OnPopupSceneLoaded(AsyncOperation op)
    {
        loginManager = FindObjectOfType<LoginManager>();
        if (loginManager != null)
        {
            loginManager.OnLoginSuccess += OnLoginSuccess;
            Debug.Log("LoginManager 찾기 성공!");
        }
        else
        {
            Debug.LogError("LoginManager를 찾을 수 없음!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 클릭 또는 터치
        {
            if (!loginCompleted)
            {
                // 로그인 패널 열기
                AllUIManager.Instance.ShowLoginPanel();
            }
            else
            {
                // 로그인 완료 후 화면 클릭 → MainMenu 이동
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    private void OnLoginSuccess()
    {
        loginCompleted = true;
        Debug.Log("로그인 성공, 다음 클릭 시 MainMenu로 이동");
        AllUIManager.Instance.HideLoginPanel();
    }
}
