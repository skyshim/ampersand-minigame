using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class ToastMessage : MonoBehaviour
{
    public Text toastMessageText;
    public float toastDuration = 2f;

    private Coroutine currentToast;

    public void ShowToast(string message, Color color)
    {
        if (currentToast != null)
            StopCoroutine(currentToast);
        currentToast = StartCoroutine(ToastCoroutine(message, color));
    }

    private IEnumerator ToastCoroutine(string message, Color color)
    {
        toastMessageText.text = message;

        // ���� ���� 0 -> 1
        Color startColor = color;
        startColor.a = 1f;
        toastMessageText.color = startColor;

        yield return new WaitForSeconds(toastDuration);

        // ���ĸ� 0���� �ٿ��� �������
        Color fadeColor = toastMessageText.color;
        fadeColor.a = 0f;
        toastMessageText.color = fadeColor;

        currentToast = null;
    }
}
