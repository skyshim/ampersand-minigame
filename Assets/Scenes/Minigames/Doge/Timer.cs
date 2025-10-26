using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject dogeOverField;

    private float elapsedTime;

    void Start()
    {
        elapsedTime = 0f;
        if (dogeOverField != null)
            dogeOverField.SetActive(false); // 시작 시 숨김
    }

    void Update()
    {
        if (DogeBlock1.AllStopped)
        {
            if (dogeOverField != null && !dogeOverField.activeSelf)
                dogeOverField.SetActive(true); // GameOverField 활성화
            return; // 타이머 정지
        }

        elapsedTime += Time.deltaTime;
        if (timerText != null)
            timerText.text = elapsedTime.ToString("f3");
    }
}
