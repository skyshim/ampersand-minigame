using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : MonoBehaviour
{
    public Text scoreText;
    public Text roundText;

    private int score = 0;

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"Score: {score}";
    }

    public void SetRound(int round)
    {
        if (round <= 10)
        {
            roundText.text = $"{round}/10";
        }
        else
        {
            roundText.text = $"Round {round}";
        }
    }
}
