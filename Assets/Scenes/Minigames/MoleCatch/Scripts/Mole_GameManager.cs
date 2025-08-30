using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mole_GameManager : MonoBehaviour
{
    public static Mole_GameManager Instance;

    public int redScore = 0;
    public int blueScore = 0;

    public float gameTime = 40f;
    private float currentTime;

    public TMP_Text timerText;
    public Mole_MoleSpawner moleSpawner;

    public GameObject countdownObj;
    public GameObject resultObj;
    public TMP_Text resultText;

    public TMP_Text redScoreText1;
    public TMP_Text redScoreText2;
    public TMP_Text blueScoreText1;
    public TMP_Text blueScoreText2;

    private bool gameRunning = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    void Update()
    {
        if (!gameRunning) return;

        currentTime -= Time.deltaTime;
        timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
        {
            gameRunning = false;
            moleSpawner.StopSpawning();
            EndGame();
        }
    }

    void Start()
    {
        resultObj.SetActive(false);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        countdownObj.SetActive(true);

        yield return new WaitForSeconds(3f);

        countdownObj.SetActive(false);

        StartGame();
    }

    void StartGame()
    {
        gameRunning = true;
        currentTime = gameTime;
        moleSpawner.StartSpawning();
    }

    void EndGame()
    {
        timerText.text = "Time's Up!";

        int red = Instance.redScore;
        int blue = Instance.blueScore;

        resultObj.SetActive(true);

        if (red > blue)
            resultText.text = "빨간팀 승리!";
        else if (blue > red)
            resultText.text = "파란팀 승리!";
        else
            resultText.text = "무승부!";
    }




    public void AddScore(MoleType moleType, int score)
    {
        switch (moleType)
        {
            case MoleType.RedMole:
            case MoleType.RedBomb:
                redScore += score;
                break;
            case MoleType.BlueMole:
            case MoleType.BlueBomb:
                blueScore += score;
                break;
        }

        string redStr = redScore.ToString();
        string blueStr = blueScore.ToString();

        redScoreText1.text = redStr;
        redScoreText2.text = redStr;
        blueScoreText1.text = blueStr;
        blueScoreText2.text = blueStr;
    }
}