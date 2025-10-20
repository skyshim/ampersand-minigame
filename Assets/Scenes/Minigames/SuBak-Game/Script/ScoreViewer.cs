using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreViewer : MonoBehaviour {
    public Text ScoreText;
    public Text FinalText;
    public int nowScore = 0;

    private GM gm;


    // Start is called before the first frame update
    void Start() {
        gm = FindObjectOfType<GM>();
        ScoreText.text = "Score: " + nowScore.ToString();
    }

    // Update is called once per frame
    void Update() {
        if (nowScore < gm.score) {
            nowScore = gm.score;
            ScoreText.text = "Score: " + nowScore.ToString();
        }
    }

    public void ResetScore() {
        nowScore = 0;
        ScoreText.text = "Score: " + nowScore.ToString();
    }

    public void FinalScore() {
        FinalText.text = "Score: " + nowScore.ToString();
    }
}
