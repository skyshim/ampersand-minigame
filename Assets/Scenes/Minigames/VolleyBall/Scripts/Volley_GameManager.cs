using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Volley_GameManager : MonoBehaviour
{
    public Transform player1Spawn;
    public Transform player2Spawn;
    public Transform ball1Spawn;
    public Transform ball2Spawn;
    public TMP_Text p1ScoreText;
    public TMP_Text p2ScoreText;
    public GameObject ball;

    public int player1Score = 0;
    public int player2Score = 0;

    void Start()
    {
        UpdateScoreUI();
    }

    // Ball에서 호출
    public void BallHitGround(float ballX)
    {
        bool player1Serves = false;

        if (ballX < 0)
        {
            // Player2 점수
            player2Score++;
            player1Serves = false;
        }
        else
        {
            // Player1 점수
            player1Score++;
            player1Serves = true;
        }

        UpdateScoreUI();
        ResetRound(player1Serves);
    }

    // 라운드 초기화
    void ResetRound(bool player1Serves)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        // 공 서브 위치 지정
        if (player1Serves)
            ball.transform.position = ball1Spawn.position;
        else
            ball.transform.position = ball2Spawn.position;

        // 플레이어 위치 초기화
        GameObject p1 = GameObject.Find("Player1");
        GameObject p2 = GameObject.Find("Player2");

        if (p1 != null) p1.transform.position = player1Spawn.position;
        if (p2 != null) p2.transform.position = player2Spawn.position;
    }

    // UI 점수 갱신
    void UpdateScoreUI()
    {
        if (p1ScoreText != null) p1ScoreText.text = player1Score.ToString();
        if (p2ScoreText != null) p2ScoreText.text = player2Score.ToString();
    }
}