using UnityEngine;
using UnityEngine.SceneManagement;

public class DogeOver : MonoBehaviour
{
    private bool hasActivated = false;
    private float sceneStartTime;

    void Awake()
    {
        sceneStartTime = Time.time; // 씬 시작 시점 기록
    }

    void OnEnable()
    {
        float elapsed = Time.time - sceneStartTime;

        if (hasActivated) return;        // 이미 활성화된 경우 무시
        if (elapsed < 0.1f) return;     // 0.1초 미만이면 무시

        Time.timeScale = 0f;
        Debug.Log("Game Over Field Activated");
        hasActivated = true;
    }

    public void OnDogeRestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnDogeQuitButton()
    {
        Time.timeScale = 0.1f;
        SceneManager.LoadScene("MainMenu");
    }
}
