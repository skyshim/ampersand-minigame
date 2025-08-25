using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class GameButtonInfo
    {
        public Button button;
        public string gameId;
        public string displayName;
        public Color themeColor;

        [HideInInspector] public Image buttonImage;
        [HideInInspector] public TMP_Text buttonText;
        [HideInInspector] public Outline buttonOutline;
    }

    [Header("Button Configuration")]
    public GameButtonInfo[] gameButtons = new GameButtonInfo[5];

    private string currentSelectedGameId;
    private RankingManager rankingManager;
    public System.Action<string> OnGameSelected;

    void Awake()
    {
        rankingManager = FindObjectOfType<RankingManager>();
        InitializeButtons();
    }

    void Start()
    {
        if (gameButtons.Length > 0 && gameButtons[0].button != null)
            SelectGame(gameButtons[0].gameId);
    }

    void InitializeButtons()
    {
        foreach (var buttonInfo in gameButtons)
        {
            if (buttonInfo.button == null) continue;

            buttonInfo.buttonImage = buttonInfo.button.GetComponent<Image>();
            buttonInfo.buttonText = buttonInfo.button.GetComponentInChildren<TMP_Text>();
            buttonInfo.buttonOutline = buttonInfo.button.GetComponent<Outline>();
            if (buttonInfo.buttonOutline == null)
                buttonInfo.buttonOutline = buttonInfo.button.gameObject.AddComponent<Outline>();

            if (buttonInfo.buttonText != null)
                buttonInfo.buttonText.text = buttonInfo.displayName;

            if (buttonInfo.buttonImage != null)
                buttonInfo.buttonImage.color = buttonInfo.themeColor * 0.7f;

            string gameId = buttonInfo.gameId;
            buttonInfo.button.onClick.RemoveAllListeners();
            buttonInfo.button.onClick.AddListener(() => SelectGame(gameId));

            buttonInfo.button.gameObject.SetActive(true);
        }
    }

    public void SelectGame(string gameId)
    {
        currentSelectedGameId = gameId;
        UpdateButtonStates();
        rankingManager?.LoadRankingForGame(gameId);
        OnGameSelected?.Invoke(gameId);
        Debug.Log($"∞‘¿” º±≈√µ : {gameId}");
    }

    void UpdateButtonStates()
    {
        foreach (var buttonInfo in gameButtons)
        {
            if (buttonInfo.button == null) continue;

            bool isSelected = buttonInfo.gameId == currentSelectedGameId;
            if (isSelected) ApplySelectedStyle(buttonInfo);
            else ApplyNormalStyle(buttonInfo);
        }
    }

    void ApplySelectedStyle(GameButtonInfo buttonInfo)
    {
        if (buttonInfo.buttonImage != null) buttonInfo.buttonImage.color = buttonInfo.themeColor;
        if (buttonInfo.buttonOutline != null)
        {
            buttonInfo.buttonOutline.effectColor = Color.white;
            buttonInfo.buttonOutline.effectDistance = new Vector2(3, 3);
        }
        buttonInfo.button.transform.localScale = Vector3.one * 1.05f;
        if (buttonInfo.buttonText != null) buttonInfo.buttonText.fontStyle = FontStyles.Bold;
    }

    void ApplyNormalStyle(GameButtonInfo buttonInfo)
    {
        if (buttonInfo.buttonImage != null) buttonInfo.buttonImage.color = buttonInfo.themeColor * 0.7f;
        if (buttonInfo.buttonOutline != null)
        {
            buttonInfo.buttonOutline.effectColor = Color.clear;
            buttonInfo.buttonOutline.effectDistance = Vector2.zero;
        }
        buttonInfo.button.transform.localScale = Vector3.one;
        if (buttonInfo.buttonText != null) buttonInfo.buttonText.fontStyle = FontStyles.Normal;
    }
}
