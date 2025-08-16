using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    private bool loginCompleted = false;
    private LoginManager loginManager;

    private void Awake()
    {
        // Additive �� �ε�, �Ϸ� �ݹ� ���
        SceneManager.LoadSceneAsync("PopupScene", LoadSceneMode.Additive).completed += OnPopupSceneLoaded;
    }

    private void OnPopupSceneLoaded(AsyncOperation op)
    {
        loginManager = FindObjectOfType<LoginManager>();
        if (loginManager != null)
        {
            loginManager.OnLoginSuccess += OnLoginSuccess;
            Debug.Log("LoginManager ã�� ����!");
        }
        else
        {
            Debug.LogError("LoginManager�� ã�� �� ����!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Ŭ�� �Ǵ� ��ġ
        {
            if (!loginCompleted)
            {
                // �α��� �г� ����
                AllUIManager.Instance.ShowLoginPanel();
            }
            else
            {
                // �α��� �Ϸ� �� ȭ�� Ŭ�� �� MainMenu �̵�
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    private void OnLoginSuccess()
    {
        loginCompleted = true;
        Debug.Log("�α��� ����, ���� Ŭ�� �� MainMenu�� �̵�");
        AllUIManager.Instance.HideLoginPanel();
    }
}
