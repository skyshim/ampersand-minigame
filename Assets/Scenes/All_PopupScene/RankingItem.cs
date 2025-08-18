using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingItem : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Text rankText;           // ���� �ؽ�Ʈ
    public TMP_Text playerNameText;     // �÷��̾� �̸� �ؽ�Ʈ
    public TMP_Text scoreText;          // ���� �ؽ�Ʈ

    [Header("Visual Settings")]
    public Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    public Color firstPlaceColor = new Color(1f, 0.84f, 0f, 0.9f);  // ���
    public Color secondPlaceColor = new Color(0.75f, 0.75f, 0.75f, 0.9f); // �ǹ�
    public Color thirdPlaceColor = new Color(0.8f, 0.5f, 0.2f, 0.9f);     // �����
    
    private RankingManager.RankingData currentData;

    public void SetData(RankingManager.RankingData data)
    {
        currentData = data;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (currentData == null) return;

        // ���� ����
        if (rankText != null)
        {
            rankText.text = currentData.rank.ToString();
        }

        // �÷��̾� �̸� ����
        if (playerNameText != null)
        {
            playerNameText.text = currentData.playerName;
        }

        // ���� ���� (õ ���� ������ �߰�)
        if (scoreText != null)
        {
            scoreText.text = currentData.score.ToString("N0");
        }

        // ������ ��Ÿ�� ����
        ApplyRankStyle();
    }

    void ApplyRankStyle()
    {
        Color targetColor = normalColor;

        // ������ ���� ����
        switch (currentData.rank)
        {
            case 1:
                targetColor = firstPlaceColor;
                break;
            case 2:
                targetColor = secondPlaceColor;
                break;
            case 3:
                targetColor = thirdPlaceColor;
                break;
            default:
                targetColor = normalColor;
                break;
        }

        // �ؽ�Ʈ ���� ����
        Color textColor = GetTextColor(targetColor);

        if (rankText != null)
        {
            rankText.color = textColor;
            rankText.fontStyle = currentData.rank <= 3 ? FontStyles.Bold : FontStyles.Normal;
        }

        if (playerNameText != null)
        {
            playerNameText.color = textColor;
            playerNameText.fontStyle = currentData.isMyRecord ? FontStyles.Bold : FontStyles.Normal;
        }

        if (scoreText != null)
        {
            scoreText.color = textColor;
            scoreText.fontStyle = currentData.isMyRecord ? FontStyles.Bold : FontStyles.Normal;
        }
    }

    Color GetTextColor(Color backgroundColor)
    {
        // ������ ������ ���� �ؽ�Ʈ, ��ο�� �� �ؽ�Ʈ
        float brightness = (backgroundColor.r + backgroundColor.g + backgroundColor.b) / 3f;
        return brightness > 0.5f ? Color.black : Color.white;
    }

    // �ִϸ��̼� ȿ�� (���û���)
    void Start()
    {
        // �������� ������ �� ���̵��� ȿ��
        StartCoroutine(FadeInEffect());
    }

    System.Collections.IEnumerator FadeInEffect()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;

        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    // Ŭ�� �̺�Ʈ (�� ���� ���� ��)
    public void OnItemClicked()
    {
        Debug.Log($"��ŷ ������ Ŭ��: {currentData.playerName} - {currentData.score}��");

        // TODO: �� ���� �˾� ǥ�� ���� ��� �߰�
        // ��: �ش� �÷��̾��� ���� ��� �󼼺���, �÷��� ���� ���� ��
    }

    // ������ ����Ǿ��� �� �ִϸ��̼� ȿ��
    public void AnimateRankChange(int newRank)
    {
        StartCoroutine(RankChangeAnimation(newRank));
    }

    System.Collections.IEnumerator RankChangeAnimation(int newRank)
    {
        // ���� �������� �� ������ ����Ǵ� �ִϸ��̼�
        Vector3 originalScale = transform.localScale;

        // ũ�� ��ȭ �ִϸ��̼�
        transform.localScale = originalScale * 1.2f;

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Lerp(1.2f, 1f, elapsedTime / duration);
            transform.localScale = originalScale * scale;
            yield return null;
        }

        transform.localScale = originalScale;

        // ���� ������Ʈ
        currentData.rank = newRank;
        UpdateDisplay();
    }
}