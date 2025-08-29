using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Volley_GameManager : MonoBehaviour
{
    public Transform player1Spawn;
    public Transform player2Spawn;
    public Transform ball1Spawn;
    public Transform ball2Spawn;
    public TMP_Text p1ScoreText;
    public TMP_Text p2ScoreText;
    public GameObject ball;

    public GameObject countdownPanel;
    public TMP_Text countdownText;

    public GameObject gameOverPanel;
    public TMP_Text winnerText;
    public Button retryButton;
    public Button homeButton;

    public int player1Score = 0;
    public int player2Score = 0;

    void Start()
    {
        retryButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        homeButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu")); // ���� �޴� �� �̸� Ȯ��

        gameOverPanel.SetActive(false);
        UpdateScoreUI();
        StartCoroutine(GameStartCountdown());
    }

    // Ball���� ȣ��
    public void BallHitGround(float ballX)
    {
        bool player1Serves = false;

        if (ballX < 0)
        {
            // Player2 ����
            player2Score++;
            player1Serves = false;
        }
        else
        {
            // Player1 ����
            player1Score++;
            player1Serves = true;
        }

        UpdateScoreUI();
        StartCoroutine(EndRound(player1Serves));
    }
    void GameOver(int winner)
    {
        gameOverPanel.SetActive(true);
        if (winner == 1) winnerText.text = "�Ķ��� �¸�!";
        else if (winner == 2) winnerText.text = "������ �¸�!";
    }

    // ���� �ʱ�ȭ
    IEnumerator EndRound(bool player1Serves)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        // 1�� ���
        yield return new WaitForSeconds(1f);
        if (player1Score >= 5)
        {
            GameOver(1);
        }
        else if (player2Score >= 5)
        {
            GameOver(2);
        }
        else
        {
            StartRound(player1Serves);
        }
        // ���� ���� ����

        
    }

    IEnumerator GameStartCountdown()
    {
        // ī��Ʈ�ٿ� Panel �ѱ�
        countdownPanel.SetActive(true);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        // ī��Ʈ�ٿ� (3,2,1,����!)
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(0.7f);
        }
        countdownText.text = "����!";
        yield return new WaitForSeconds(0.5f);

        // Panel ����
        countdownPanel.SetActive(false);
        rb.isKinematic = false;

        // ù ���� ���� (Player1�� �����Ѵٰ� ����)
        StartRound(true);
    }

    public void StartRound(bool player1Serves)
    {
        // �÷��̾� ��ġ �ʱ�ȭ
        GameObject p1 = GameObject.Find("Player1");
        GameObject p2 = GameObject.Find("Player2");
        if (p1 != null) p1.transform.position = player1Spawn.position;
        if (p2 != null) p2.transform.position = player2Spawn.position;

        // �� ����
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        

        if (player1Serves)
            ball.transform.position = ball1Spawn.position;
        else
            ball.transform.position = ball2Spawn.position;
    }

    // UI ���� ����
    void UpdateScoreUI()
    {
        if (p1ScoreText != null) p1ScoreText.text = player1Score.ToString();
        if (p2ScoreText != null) p2ScoreText.text = player2Score.ToString();
    }
}