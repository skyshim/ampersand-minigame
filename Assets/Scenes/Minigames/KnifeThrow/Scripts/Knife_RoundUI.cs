using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knife_RoundUI : MonoBehaviour
{
    public Text scoreText;
    public Text roundText;

    public int score = 0;

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"Score: {score}";
    }
    public int LoadScore()
    {
        return score;
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
