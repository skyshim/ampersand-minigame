using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingItem : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Text rankText;           // 순위 텍스트
    public TMP_Text playerNameText;     // 플레이어 이름 텍스트
    public TMP_Text scoreText;          // 점수 텍스트

    [Header("Visual Settings")]
    public Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    public Color firstPlaceColor = new Color(1f, 0.84f, 0f, 0.9f);  // 골드
    public Color secondPlaceColor = new Color(0.75f, 0.75f, 0.75f, 0.9f); // 실버
    public Color thirdPlaceColor = new Color(0.8f, 0.5f, 0.2f, 0.9f);     // 브론즈
    
    private RankingManager.RankingData currentData;

    public void SetData(RankingManager.RankingData data)
    {
        currentData = data;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (currentData == null) return;

        // 순위 설정
        if (rankText != null)
        {
            rankText.text = currentData.rank.ToString();
        }

        // 플레이어 이름 설정
        if (playerNameText != null)
        {
            playerNameText.text = currentData.playerName;
        }

        // 점수 설정 (천 단위 구분자 추가)
        if (scoreText != null)
        {
            scoreText.text = currentData.score.ToString("N0");
        }

        // 순위별 스타일 적용
        ApplyRankStyle();
    }

    void ApplyRankStyle()
    {
        Color targetColor = normalColor;

        // 순위별 색상 설정
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

        // 텍스트 색상 설정
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
        // 배경색이 밝으면 검은 텍스트, 어두우면 흰 텍스트
        float brightness = (backgroundColor.r + backgroundColor.g + backgroundColor.b) / 3f;
        return brightness > 0.5f ? Color.black : Color.white;
    }

    // 애니메이션 효과 (선택사항)
    void Start()
    {
        // 아이템이 생성될 때 페이드인 효과
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

    // 클릭 이벤트 (상세 정보 보기 등)
    public void OnItemClicked()
    {
        Debug.Log($"랭킹 아이템 클릭: {currentData.playerName} - {currentData.score}점");

        // TODO: 상세 정보 팝업 표시 등의 기능 추가
        // 예: 해당 플레이어의 게임 기록 상세보기, 플레이 영상 보기 등
    }

    // 순위가 변경되었을 때 애니메이션 효과
    public void AnimateRankChange(int newRank)
    {
        StartCoroutine(RankChangeAnimation(newRank));
    }

    System.Collections.IEnumerator RankChangeAnimation(int newRank)
    {
        // 현재 순위에서 새 순위로 변경되는 애니메이션
        Vector3 originalScale = transform.localScale;

        // 크기 변화 애니메이션
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

        // 순위 업데이트
        currentData.rank = newRank;
        UpdateDisplay();
    }
}