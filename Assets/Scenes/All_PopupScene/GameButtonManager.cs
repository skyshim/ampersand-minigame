using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class GameButtonInfo
    {
        public Button button;           // 씬에 배치된 버튼
        public string gameId;          // 게임 ID
        public string displayName;     // 표시될 게임 이름
        public Color themeColor;       // 테마 색상

        [HideInInspector]
        public Image buttonImage;      // 버튼 이미지 (자동 할당)
        [HideInInspector]
        public TMP_Text buttonText;    // 버튼 텍스트 (자동 할당)
        [HideInInspector]
        public Outline buttonOutline;  // 버튼 아웃라인 (자동 생성)
    }

    [Header("Button Configuration")]
    public GameButtonInfo[] gameButtons = new GameButtonInfo[5]; // Inspector에서 설정

    private string currentSelectedGameId;
    private RankingManager rankingManager;

    // 게임 선택 이벤트
    public System.Action<string> OnGameSelected;

    void Awake()
    {
        rankingManager = FindObjectOfType<RankingManager>();
        InitializeButtons();
    }

    void Start()
    {
        // 첫 번째 게임을 기본 선택
        if (gameButtons.Length > 0 && gameButtons[0].button != null)
        {
            SelectGame(gameButtons[0].gameId);
        }
    }

    void InitializeButtons()
    {
        for (int i = 0; i < gameButtons.Length; i++)
        {
            GameButtonInfo buttonInfo = gameButtons[i];

            if (buttonInfo.button == null) continue;

            // 컴포넌트 자동 할당
            buttonInfo.buttonImage = buttonInfo.button.GetComponent<Image>();
            buttonInfo.buttonText = buttonInfo.button.GetComponentInChildren<TMP_Text>();
            buttonInfo.buttonOutline = buttonInfo.button.GetComponent<Outline>();

            // Outline 컴포넌트가 없으면 추가
            if (buttonInfo.buttonOutline == null)
            {
                buttonInfo.buttonOutline = buttonInfo.button.gameObject.AddComponent<Outline>();
            }

            // 버튼 텍스트 설정
            if (buttonInfo.buttonText != null)
            {
                buttonInfo.buttonText.text = buttonInfo.displayName;
            }

            // 버튼 초기 색상 설정
            if (buttonInfo.buttonImage != null)
            {
                buttonInfo.buttonImage.color = buttonInfo.themeColor * 0.7f;
            }

            // 버튼 클릭 이벤트 연결
            string gameId = buttonInfo.gameId; // 람다식용 지역 변수
            buttonInfo.button.onClick.RemoveAllListeners();
            buttonInfo.button.onClick.AddListener(() => SelectGame(gameId));

            // 버튼 활성화
            buttonInfo.button.gameObject.SetActive(true);
        }
    }

    public void SelectGame(string gameId)
    {
        currentSelectedGameId = gameId;

        // 버튼 상태 업데이트
        UpdateButtonStates();

        // 랭킹 매니저에 게임 선택 알림
        if (rankingManager != null)
        {
            rankingManager.LoadRankingForGame(gameId);
        }

        // 이벤트 발생
        OnGameSelected?.Invoke(gameId);

        Debug.Log($"게임 선택됨: {gameId}");
    }

    void UpdateButtonStates()
    {
        for (int i = 0; i < gameButtons.Length; i++)
        {
            GameButtonInfo buttonInfo = gameButtons[i];

            if (buttonInfo.button == null) continue;

            bool isSelected = buttonInfo.gameId == currentSelectedGameId;

            if (isSelected)
            {
                // 선택된 버튼 스타일
                ApplySelectedStyle(buttonInfo);
            }
            else
            {
                // 선택되지 않은 버튼 스타일
                ApplyNormalStyle(buttonInfo);
            }
        }
    }

    void ApplySelectedStyle(GameButtonInfo buttonInfo)
    {
        // 배경색 - 원래 색상
        if (buttonInfo.buttonImage != null)
        {
            buttonInfo.buttonImage.color = buttonInfo.themeColor;
        }

        // 테두리 강조
        if (buttonInfo.buttonOutline != null)
        {
            buttonInfo.buttonOutline.effectColor = Color.white;
            buttonInfo.buttonOutline.effectDistance = new Vector2(3, 3);
        }

        // 크기 확대
        buttonInfo.button.transform.localScale = Vector3.one * 1.05f;

        // 텍스트 굵게
        if (buttonInfo.buttonText != null)
        {
            buttonInfo.buttonText.fontStyle = FontStyles.Bold;
        }
    }

    void ApplyNormalStyle(GameButtonInfo buttonInfo)
    {
        // 배경색 - 어둡게
        if (buttonInfo.buttonImage != null)
        {
            buttonInfo.buttonImage.color = buttonInfo.themeColor * 0.7f;
        }

        // 테두리 제거
        if (buttonInfo.buttonOutline != null)
        {
            buttonInfo.buttonOutline.effectColor = Color.clear;
            buttonInfo.buttonOutline.effectDistance = Vector2.zero;
        }

        // 원래 크기
        buttonInfo.button.transform.localScale = Vector3.one;

        // 텍스트 보통
        if (buttonInfo.buttonText != null)
        {
            buttonInfo.buttonText.fontStyle = FontStyles.Normal;
        }
    }

    // 특정 게임 버튼 활성화/비활성화
    public void SetGameButtonActive(string gameId, bool active)
    {
        GameButtonInfo buttonInfo = GetButtonInfo(gameId);
        if (buttonInfo != null && buttonInfo.button != null)
        {
            buttonInfo.button.gameObject.SetActive(active);
        }
    }

    // 게임 정보 가져오기
    public GameButtonInfo GetButtonInfo(string gameId)
    {
        for (int i = 0; i < gameButtons.Length; i++)
        {
            if (gameButtons[i].gameId == gameId)
            {
                return gameButtons[i];
            }
        }
        return null;
    }

    // 현재 선택된 게임 ID 반환
    public string GetCurrentSelectedGameId()
    {
        return currentSelectedGameId;
    }

    // 게임 버튼 색상 변경
    public void UpdateGameButtonColor(string gameId, Color newColor)
    {
        GameButtonInfo buttonInfo = GetButtonInfo(gameId);
        if (buttonInfo != null)
        {
            buttonInfo.themeColor = newColor;

            // 현재 선택 상태에 따라 색상 적용
            if (buttonInfo.gameId == currentSelectedGameId)
            {
                ApplySelectedStyle(buttonInfo);
            }
            else
            {
                ApplyNormalStyle(buttonInfo);
            }
        }
    }

    // 게임 버튼 텍스트 변경
    public void UpdateGameButtonText(string gameId, string newText)
    {
        GameButtonInfo buttonInfo = GetButtonInfo(gameId);
        if (buttonInfo != null)
        {
            buttonInfo.displayName = newText;
            if (buttonInfo.buttonText != null)
            {
                buttonInfo.buttonText.text = newText;
            }
        }
    }

    // 모든 활성화된 게임 ID 리스트 반환
    public List<string> GetActiveGameIds()
    {
        List<string> activeGameIds = new List<string>();

        for (int i = 0; i < gameButtons.Length; i++)
        {
            if (gameButtons[i].button != null &&
                gameButtons[i].button.gameObject.activeInHierarchy &&
                !string.IsNullOrEmpty(gameButtons[i].gameId))
            {
                activeGameIds.Add(gameButtons[i].gameId);
            }
        }

        return activeGameIds;
    }
}