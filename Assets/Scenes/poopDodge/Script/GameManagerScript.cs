using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public bool isGameOver = false;
    public bool isGameStart = false;

    public int gamemode = 1;
    public int cnt = 0;

    public Button gameBtn;
    public TMP_Text gameBtnTxt;
    public TMP_Dropdown gamemodeDrp;

    // Start is called before the first frame update
    void Start()
    {
        gameBtn.onClick.AddListener(gameStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            gameBtnTxt.text = "Restart?";
        }
    }
    void gameStart()
    {
        isGameStart = true;
        gameBtn.gameObject.SetActive(false);
        gamemodeDrp.gameObject.SetActive(false);
        gamemode = gamemodeDrp.value + 1;
        Debug.Log(gamemode);
    }
}
