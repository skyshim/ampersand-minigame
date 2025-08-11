using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AllUIManager : MonoBehaviour
{
    public static AllUIManager Instance;

    [Header("Result Panel")]
    public GameObject resultPanel;
    public Text titleText;
    public Button homeButton;
    public Button retryButton;
    public Button rankButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowResult(string title, UnityAction onRetry)
    {
        titleText.text = title;
        resultPanel.SetActive(true);

        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(() =>
        {
            resultPanel.SetActive(false);
            onRetry.Invoke();
        });

        homeButton.onClick.RemoveAllListeners();
        homeButton.onClick.AddListener(() =>
        {
            // Ȩ ȭ������ �̵�
            SceneManager.LoadScene("HomeScene");
        });

        rankButton.onClick.RemoveAllListeners();
        rankButton.onClick.AddListener(() =>
        {
            // ��ŷ ��� ȣ��
            Debug.Log("��ŷ ����");
        });
    }

    public void HideResult()
    {
        resultPanel.SetActive(false);
    }
}
