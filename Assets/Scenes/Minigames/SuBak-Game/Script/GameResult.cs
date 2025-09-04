using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public bool isGameOver = false;

    private GM gm;
    private Hand hand;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GM>();
        hand = FindObjectOfType<Hand>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    void ResetGame() {
        isGameOver = false;

        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit_SuBak-Game");
        foreach (GameObject fruit in fruits) {
            Destroy(fruit);
        }

        hand.ResetGame();
    }


}
