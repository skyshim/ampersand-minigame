using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour {

    public int fruitLevel = 1; // 1~10
    private Rigidbody2D rb;
    private GM gm;
    private Hand hand;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GM>();
        hand = FindObjectOfType<Hand>();

        if (gm.spawnType) { // 병합으로 생성
            fruitLevel = gm.mergeLevel;
            gm.spawnType = false;
            gm.mergeLevel = 1;
        }
        else { // 클릭으로 생성
            fruitLevel = hand.spawnLevel;
        }
        gameObject.transform.localScale = new Vector3(gm.fruitDiameter[fruitLevel - 1], gm.fruitDiameter[fruitLevel - 1], 1);

    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag(gameObject.tag)) return;
        if (fruitLevel == collision.gameObject.GetComponent<FruitController>().fruitLevel) {
            gm.mergeSig = true;
            gm.mergeLevel = fruitLevel + 1;

            float mergeX = (transform.position.x + collision.transform.position.x) / 2;
            float mergeY = (transform.position.y + collision.transform.position.y) / 2;
            gm.mergePos = new Vector2(mergeX, mergeY);

            Destroy(gameObject);
        }

    }
}