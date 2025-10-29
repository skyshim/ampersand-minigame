using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public bool isGameOver = false;
    public bool isGameClear = false;

    private GM gm;
    private ScoreViewer sv;
    private UM um;
    private Hand hand;
    public int score = 0;


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GM>();
        hand = FindObjectOfType<Hand>();
        um = FindObjectOfType<UM>();
        sv = FindObjectOfType<ScoreViewer>();
    }


    public void ResetGame() {
        if (!isGameOver) return;
        if (isGameClear) isGameClear = false;

        isGameOver = false;

        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit_SuBak-Game");
        foreach (GameObject fruit in fruits) {
            Destroy(fruit);
        }

        gm.isRankingSubmitted = false;
        score = 0;
        sv.ResetScore();
        hand.ResetGame();
    }

    public void GameOver() {
        isGameOver = true;
        um.OpenUI(2);
        sv.FinalScore();
    }

    public void GameClear() {
        isGameClear = true;
        isGameOver = true;
        um.OpenUI(3);
        sv.FinalScore();
    }
}
