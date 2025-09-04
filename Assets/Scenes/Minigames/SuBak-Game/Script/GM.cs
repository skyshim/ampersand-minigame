using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

    public GameObject fruitprefab;
    public int mergeLevel = 1;

    [HideInInspector]
    public bool mergeSig = false;
    public Vector2 mergePos;
    public bool spawnType = false; // true : merge, false : click
    public float[] fruitDiameter = { 0.3f, 0.6f, 0.9f, 1.2f, 1.5f, 1.8f, 2.1f, 2.4f, 2.7f, 3.0f };
    public float[] fruitMass = { 0.01f, 0.05f, 0.1f, 0.2f, 0.35f, 0.55f, 0.8f, 1.1f, 1.45f, 1.85f };

    private float previousTime;
    private GameResult gr;

    // Start is called before the first frame update
    void Start() {
        mergeLevel = SpawnLevel();
        previousTime = Time.time;
        gr = FindObjectOfType<GameResult>();
    }

    // 병합, 게임 끝
    void Update() {
        
        if (mergeSig) {
            spawnType = true; // 병합으로 생성
            Instantiate(fruitprefab, mergePos, Quaternion.identity);
            mergeSig = false;
        }
    }


    // 랜덤 생성 레벨
    int SpawnLevel() {
        int r = Random.Range(1, 7);
        if (r <= 3) return 1;
        else if (r <= 5) return 2;
        else return 3;
    }

    public void Clear() {
        gr.isGameOver = true;
        
    }
}
