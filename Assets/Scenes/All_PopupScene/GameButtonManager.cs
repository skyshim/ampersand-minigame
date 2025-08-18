using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class GameButtonInfo
    {
        public Button button;           // ���� ��ġ�� ��ư
        public string gameId;          // ���� ID
        public string displayName;     // ǥ�õ� ���� �̸�
        public Color themeColor;       // �׸� ����

        [HideInInspector]
        public Image buttonImage;      // ��ư �̹��� (�ڵ� �Ҵ�)
        [HideInInspector]
        public TMP_Text buttonText;    // ��ư �ؽ�Ʈ (�ڵ� �Ҵ�)
        [HideInInspector]
        public Outline buttonOutline;  // ��ư �ƿ����� (�ڵ� ����)
    }

    [Header("Button Configuration")]
    public GameButtonInfo[] gameButtons = new GameButtonInfo[5]; // Inspector���� ����

    private string currentSelectedGameId;
    private RankingManager rankingManager;

    // ���� ���� �̺�Ʈ
    public System.Action<string> OnGameSelected;

    void Awake()
    {
        rankingManager = FindObjectOfType<RankingManager>();
        InitializeButtons();
    }

    void Start()
    {
        // ù ��° ������ �⺻ ����
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

            // ������Ʈ �ڵ� �Ҵ�
            buttonInfo.buttonImage = buttonInfo.button.GetComponent<Image>();
            buttonInfo.buttonText = buttonInfo.button.GetComponentInChildren<TMP_Text>();
            buttonInfo.buttonOutline = buttonInfo.button.GetComponent<Outline>();

            // Outline ������Ʈ�� ������ �߰�
            if (buttonInfo.buttonOutline == null)
            {
                buttonInfo.buttonOutline = buttonInfo.button.gameObject.AddComponent<Outline>();
            }

            // ��ư �ؽ�Ʈ ����
            if (buttonInfo.buttonText != null)
            {
                buttonInfo.buttonText.text = buttonInfo.displayName;
            }

            // ��ư �ʱ� ���� ����
            if (buttonInfo.buttonImage != null)
            {
                buttonInfo.buttonImage.color = buttonInfo.themeColor * 0.7f;
            }

            // ��ư Ŭ�� �̺�Ʈ ����
            string gameId = buttonInfo.gameId; // ���ٽĿ� ���� ����
            buttonInfo.button.onClick.RemoveAllListeners();
            buttonInfo.button.onClick.AddListener(() => SelectGame(gameId));

            // ��ư Ȱ��ȭ
            buttonInfo.button.gameObject.SetActive(true);
        }
    }

    public void SelectGame(string gameId)
    {
        currentSelectedGameId = gameId;

        // ��ư ���� ������Ʈ
        UpdateButtonStates();

        // ��ŷ �Ŵ����� ���� ���� �˸�
        if (rankingManager != null)
        {
            rankingManager.LoadRankingForGame(gameId);
        }

        // �̺�Ʈ �߻�
        OnGameSelected?.Invoke(gameId);

        Debug.Log($"���� ���õ�: {gameId}");
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
                // ���õ� ��ư ��Ÿ��
                ApplySelectedStyle(buttonInfo);
            }
            else
            {
                // ���õ��� ���� ��ư ��Ÿ��
                ApplyNormalStyle(buttonInfo);
            }
        }
    }

    void ApplySelectedStyle(GameButtonInfo buttonInfo)
    {
        // ���� - ���� ����
        if (buttonInfo.buttonImage != null)
        {
            buttonInfo.buttonImage.color = buttonInfo.themeColor;
        }

        // �׵θ� ����
        if (buttonInfo.buttonOutline != null)
        {
            buttonInfo.buttonOutline.effectColor = Color.white;
            buttonInfo.buttonOutline.effectDistance = new Vector2(3, 3);
        }

        // ũ�� Ȯ��
        buttonInfo.button.transform.localScale = Vector3.one * 1.05f;

        // �ؽ�Ʈ ����
        if (buttonInfo.buttonText != null)
        {
            buttonInfo.buttonText.fontStyle = FontStyles.Bold;
        }
    }

    void ApplyNormalStyle(GameButtonInfo buttonInfo)
    {
        // ���� - ��Ӱ�
        if (buttonInfo.buttonImage != null)
        {
            buttonInfo.buttonImage.color = buttonInfo.themeColor * 0.7f;
        }

        // �׵θ� ����
        if (buttonInfo.buttonOutline != null)
        {
            buttonInfo.buttonOutline.effectColor = Color.clear;
            buttonInfo.buttonOutline.effectDistance = Vector2.zero;
        }

        // ���� ũ��
        buttonInfo.button.transform.localScale = Vector3.one;

        // �ؽ�Ʈ ����
        if (buttonInfo.buttonText != null)
        {
            buttonInfo.buttonText.fontStyle = FontStyles.Normal;
        }
    }

    // Ư�� ���� ��ư Ȱ��ȭ/��Ȱ��ȭ
    public void SetGameButtonActive(string gameId, bool active)
    {
        GameButtonInfo buttonInfo = GetButtonInfo(gameId);
        if (buttonInfo != null && buttonInfo.button != null)
        {
            buttonInfo.button.gameObject.SetActive(active);
        }
    }

    // ���� ���� ��������
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

    // ���� ���õ� ���� ID ��ȯ
    public string GetCurrentSelectedGameId()
    {
        return currentSelectedGameId;
    }

    // ���� ��ư ���� ����
    public void UpdateGameButtonColor(string gameId, Color newColor)
    {
        GameButtonInfo buttonInfo = GetButtonInfo(gameId);
        if (buttonInfo != null)
        {
            buttonInfo.themeColor = newColor;

            // ���� ���� ���¿� ���� ���� ����
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

    // ���� ��ư �ؽ�Ʈ ����
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

    // ��� Ȱ��ȭ�� ���� ID ����Ʈ ��ȯ
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