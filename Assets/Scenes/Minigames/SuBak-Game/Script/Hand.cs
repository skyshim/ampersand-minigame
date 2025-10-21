using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
    public int nextSpawnLevel = 1;
    public int spawnLevel = 1; // 1~10
    private float formerclick;
    public float clickDelay = 0.1f; // 클릭 딜레이
    public float mergeDelay;
    public bool dropSig = false;

    public bool isleft = false;
    public bool isright = false;
    public bool isdown = false;

    public GameObject fruitPrefab;
    public GameObject spawnPoint;
    private GM gm;
    private GameResult gr;
    [SerializeField] spawnerAnimation anim;

    // 위치 초기화
    void Start() {
        gm = FindObjectOfType<GM>();
        gr = FindObjectOfType<GameResult>();
        formerclick = Time.time;
    }


    // 플레이어 인풋
    void Update() {
        if (gr.isGameOver) return;

        if (isleft && !isright && transform.position.x > -2) transform.Translate(new Vector2(-1, 0) * Time.deltaTime * 5);
        if (isright && !isleft && transform.position.x < 2) transform.Translate(new Vector2(1, 0) * Time.deltaTime * 5);
        if (isdown && Time.time - formerclick > clickDelay) Spawn();
        if (Time.time - formerclick < clickDelay) isdown = false;
    }


    // 과일 랜덤 생성
    private void Spawn() {
        formerclick = Time.time;
        isdown = false;

        spawnLevel = nextSpawnLevel;
        int r = Random.Range(1, 25);
        if (r <= 6) nextSpawnLevel = 1;
        else if (r <= 12) nextSpawnLevel = 2;
        else if (r <= 18) nextSpawnLevel = 3;
        else nextSpawnLevel = 4;

        spawnPoint.SetActive(false);
        dropSig = true;
        Invoke("SpawnerON", clickDelay);
        Debug.Log(nextSpawnLevel + "___");
        anim.ChangeShape(nextSpawnLevel);

        gm.spawnType = false; // 클릭으로 생성
        Instantiate(fruitPrefab, spawnPoint.transform.position, transform.rotation); // 이후 과일 생성
        gm.GetScore(spawnLevel);
    }


    // 다음 떨굴거 보여주기
    private void SpawnerON() {
        dropSig = false;
        spawnPoint.SetActive(true); 
    }


    // 리셋 시 초기화
    public void ResetGame() {
        spawnLevel = 1;
        nextSpawnLevel = 1;
        transform.position = new Vector2(0, transform.position.y);
        gm.score = 0;
    }

}

