using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public bool isGameOver = false;
    public bool isGameStart = false;

    public int gamemode = 1;
    public int cnt = 0;

    public Button gameBtn;
    public Button quitBtn;
    public TMP_Text gameBtnTxt;
    public TMP_Dropdown gamemodeDrp;
    public TMP_Text score;
    public TMP_Text gameScore;

    // Start is called before the first frame update
    void Start()
    {
        gameBtn.onClick.AddListener(gameStart);
        quitBtn.onClick.AddListener(quitGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            quitBtn.gameObject.SetActive(true); 
            gameBtn.gameObject.SetActive(true);
            gameBtnTxt.text = "Restart?";

            int bestScore = PlayerPrefs.GetInt("BestScore");
            if (cnt > bestScore)
            {
                bestScore = cnt;
                PlayerPrefs.SetInt("BestScore", bestScore);
            }
            gameScore.gameObject.SetActive(true);
            gameScore.text = "Best score : " + (int)bestScore;
        }
        else { score.text = cnt.ToString(); }
    }
    void gameStart()
    {
        isGameStart = true;
        gameBtn.gameObject.SetActive(false);
        gamemodeDrp.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        gamemode = gamemodeDrp.value + 1;
        gameBtn.onClick.RemoveAllListeners();
        gameBtn.onClick.AddListener(restart);
    }

    void restart() { SceneManager.LoadScene("poopDodge"); }

    void quitGame() { Application.Quit(); }
}
