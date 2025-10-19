using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class to50_Button : MonoBehaviour
{
    public to50_GameManager gameManager;
    public int num;

    private TMP_Text textComponent;
    private Image background;
    private bool isSecond = false;

    private void Start()
    {
        gameManager = FindAnyObjectByType<to50_GameManager>();
        textComponent = GetComponentInChildren<TMP_Text>();
        background = GetComponent<Image>();
    }
    public void OnClicked()
    {
        Debug.Log(gameManager.currentNumber);
        if (gameManager.isCurrent(num) && gameManager.isStarted)
        {
            if (!isSecond)
            {
                int temp = gameManager.popSecond();
                textComponent.text = temp.ToString();
                num = temp;
                background.color = new Color(0.9f, 0.9f, 0.9f, 1f);

                gameManager.currentNumber += 1;
                isSecond = true;
            }
            else
            {
                background.color = new Color(1f, 1f, 1f, 0f);
                gameManager.currentNumber += 1;
                textComponent.text = "";
                if (num == 50)
                {
                    gameManager.EndGame();
                }
            }
        }
    }
}
