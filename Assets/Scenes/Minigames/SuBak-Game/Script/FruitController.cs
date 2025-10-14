using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour {

    private Rigidbody2D rb;
    private GM gm;
    private Hand hand;
    private GameResult gr;

    public int fruitLevel = 1; // 1~10
    private float spawnTime; // 생성시점
    float stayTime = 0f;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GM>();
        hand = FindObjectOfType<Hand>();
        gr = FindObjectOfType<GameResult>();

        if (gm.spawnType) { // 병합으로 생성
            fruitLevel = gm.mergeLevel;
            gm.spawnType = false;
            gm.mergeLevel = 1;
        }
        else { // 클릭으로 생성
            fruitLevel = hand.spawnLevel;
        }
        gameObject.transform.localScale = new Vector3(gm.fruitDiameter[fruitLevel - 1], gm.fruitDiameter[fruitLevel - 1], 1);
        rb.mass = gm.fruitMass[fruitLevel - 1];
        spawnTime = Time.time;
        Pause(0.1f);
    }


    // 스크립트꺼서 잠시 대기
    IEnumerator Pause(float seconds) {
        // 원하는 동작 중단
        enabled = false;  // 이 스크립트 자체를 끔
        if (gameObject == null) yield break;
        yield return new WaitForSeconds(seconds);
        if (gameObject == null) yield break;
        enabled = true;   // 다시 켬
    }


    // 게임 오버 시 정지
    private void Update() {
        if (gr.isGameOver) {
            rb.simulated = false;
        } else { rb.simulated = true;}
    }

    // 과일끼리 충돌시 병합
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag(gameObject.tag)) return;
        if (spawnTime - Time.time < hand.mergeDelay) {
            StartCoroutine(Pause(hand.mergeDelay));
            StartCoroutine(collision.gameObject.GetComponent<FruitController>().Pause(hand.mergeDelay));
        }
        if (fruitLevel == collision.gameObject.GetComponent<FruitController>().fruitLevel && fruitLevel != 10) {
            gm.mergeSig = true;
            gm.mergeLevel = fruitLevel + 1;

            float mergeX = (transform.position.x + collision.transform.position.x) / 2;
            float mergeY = (transform.position.y + collision.transform.position.y) / 2;
            gm.mergePos = new Vector2(mergeX, mergeY);

            Destroy(gameObject);
        }

    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Overline_Subak-Game")) {
            stayTime += Time.deltaTime;
        }

        if (stayTime >= 1f) {
            gr.GameOver();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Overline_Subak-Game")) {
            stayTime = 0f;
        }
    }
}