using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
    private float go = 0;
    public int nextSpawnLevel = 1;
    public int spawnLevel = 1; // 1~10
    private float formerclick;
    public float clickDelay = 0.1f; // 클릭 딜레이
    public float mergeDelay;
    public bool dropSig = false;

    public GameObject fruitPrefab;
    public GameObject spawnPoint;
    private GM gm;
    private GameResult gr;

    // 위치 초기화
    void Start() {
        gm = FindObjectOfType<GM>();
        gr = FindObjectOfType<GameResult>();
        spawnPoint.transform.localScale = new Vector3(gm.fruitDiameter[spawnLevel - 1], gm.fruitDiameter[spawnLevel - 1], 1);
        formerclick = Time.time;
    }


    // 플레이어 인풋
    void Update() {
        if (gr.isGameOver) return;
        // 이동 명령
        if (transform.position.x < -2) {
            go = Input.GetAxisRaw("Horizontal");
            if (go < 0) go = 0;
            transform.Translate(new Vector2(go, 0) * Time.deltaTime * 5);
        }
        else if (transform.position.x > 2) {
            go = Input.GetAxisRaw("Horizontal");
            if (go > 0) go = 0;
            transform.Translate(new Vector2(go, 0) * Time.deltaTime * 5);
        }
        else {
            go = Input.GetAxisRaw("Horizontal");
            transform.Translate(new Vector2(go, 0) * Time.deltaTime * 5);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time - formerclick > clickDelay) {
            Spawn();
        }
    }


    // 과일 랜덤 생성
    private void Spawn() {
        formerclick = Time.time;

        spawnLevel = nextSpawnLevel;
        int r = Random.Range(1, 25);
        if (r <= 6) nextSpawnLevel = 1;
        else if (r <= 12) nextSpawnLevel = 2;
        else if (r <= 18) nextSpawnLevel = 3;
        else if (r == 22) nextSpawnLevel = 4;
        else nextSpawnLevel = 5;

        spawnPoint.SetActive(false);
        dropSig = true;
        Invoke("SizeChange", clickDelay);



        gm.spawnType = false; // 클릭으로 생성
        Instantiate(fruitPrefab, spawnPoint.transform.position, transform.rotation); // 이후 과일 생성
        gm.GetScore(spawnLevel);
    }


    // 다음 떨굴거 보여주기
    private void SizeChange() {
        dropSig = false;
        spawnPoint.SetActive(true);
        spawnPoint.transform.localScale = new Vector3(gm.fruitDiameter[nextSpawnLevel - 1], gm.fruitDiameter[nextSpawnLevel - 1], 1);
    }


    // 리셋 시 초기화
    public void ResetGame() {
        spawnLevel = 1;
        nextSpawnLevel = 1;
        spawnPoint.transform.localScale = new Vector3(gm.fruitDiameter[spawnLevel - 1], gm.fruitDiameter[spawnLevel - 1], 1);
        transform.position = new Vector2(0, transform.position.y);
        gm.score = 0;
    }

}

