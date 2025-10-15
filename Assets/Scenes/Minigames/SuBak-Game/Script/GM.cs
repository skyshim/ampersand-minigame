using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

    public bool isItemMode = false;
    public GameObject fruitprefab;
    public int mergeLevel = 1;

    [HideInInspector]
    public bool mergeSig = false;
    public Vector2 mergePos;
    public bool spawnType = false; // true : merge, false : click
    public float[] fruitDiameter = { 0.3f, 0.5f, 0.9f, 1.1f, 1.5f, 1.7f, 2.1f, 2.3f, 2.7f, 3.0f };
    public float[] fruitMass = { 0.01f, 0.05f, 0.1f, 0.2f, 0.35f, 0.55f, 0.8f, 1.1f, 1.45f, 1.85f };
    public int[] fruitScore = { 100, 400, 900, 1600, 2500, 3600, 4900, 6400, 8100, 10000 }; // 레벨별 점수
    public int score = 0;

    private GameResult gr;
    private UM um;


    // Start is called before the first frame update
    void Start() {
        Screen.SetResolution(1080, 1920, false); // width, height, fullscreen
        Screen.orientation = ScreenOrientation.Portrait;

        mergeLevel = SpawnLevel();
        gr = FindObjectOfType<GameResult>();
        um = FindObjectOfType<UM>();

        um.OpenUI(1);
    }

    // 병합, 게임 끝
    void Update() {
        
        if (mergeSig) {
            spawnType = true; // 병합으로 생성
            Instantiate(fruitprefab, mergePos, Quaternion.identity);
            GetScore(mergeLevel);
            mergeSig = false;
        }

        if (mergeLevel == 10) {
            gr.GameClear();
        }
    }


    // 랜덤 생성 레벨
    int SpawnLevel() {
        int r = Random.Range(1, 7);
        if (r <= 3) return 1;
        else if (r <= 5) return 2;
        else return 3;
    }

    public void GetScore(int level) {
        score += fruitScore[level - 1];
        Debug.Log("Score : " + score);
    }
}
