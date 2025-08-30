using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DinoJumpManagerScript : MonoBehaviour
{
    public GameObject dino;
    public TMP_Text gameText;
    public TMP_Text scoreText;

    public int score = 0;
    public float tm = 0f;
    public float gameSpeed = 1f;
    public bool isGamestarted = false;
    public bool isGameovered = false;

    // Start is called before the first frame update
    void Start()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            gameText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(true);   
            DinoScript dinoScript = dino.GetComponent<DinoScript>();
            dinoScript.jump();
            isGamestarted = true;
        }
        if (!isGameovered && isGamestarted)
        {
            gameSpeed += Time.deltaTime * Time.deltaTime;
            tm += Time.deltaTime;
            score = (int)tm;
            scoreText.text = score.ToString();
        }
        if (isGameovered)
        {
            int bestScore = PlayerPrefs.GetInt("BestScore");
            if (score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetInt("BestScore", bestScore);
            }
            gameText.text = "Your score : " + (int)score + "\nBest score : " + (int)bestScore;
            scoreText.text = "Tap to Run Again";
            gameText.gameObject.SetActive(true);

            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                SceneManager.LoadScene("DinoJump");
            }
        }
        else { scoreText.text = score.ToString(); }
    }
}
