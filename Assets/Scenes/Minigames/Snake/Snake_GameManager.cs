using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;

public class Snake_GameManager : MonoBehaviour
{
    public Snake_GridManager gridManager;
    public Snake_Move snake;
    public Snake_FoodManager foodManager;
    public GameObject snakeHead;

    public GameObject startPanel;
    public TMP_Text scoreText;
    public TMP_Text gameOverText;

    public void StartGame(int gridCount)
    {
        // StartPanel 끄기
        startPanel.SetActive(false);

        // Grid, Snake, Food 초기화
        gridManager.gridCount = gridCount;
        foodManager.gridCount = gridCount;
        snake.gridCount = gridCount;

        gridManager.CreateGrid();
        gridManager.SetupCamera(); //첫음식생성은 스네이크에서
        scoreText.gameObject.SetActive(true);
        snakeHead.SetActive(true);
        gameOverText.text = "";
    }

    void Update()
    {
        // 점수 실시간 표시
        scoreText.text = snake.score.ToString();

        // Game Over 처리
        if (!snake.enabled && gameOverText.text == "")
        {
            gameOverText.text = "Game Over!\nFinal Score: " + snake.score;
        }
    }
}
