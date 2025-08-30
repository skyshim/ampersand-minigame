using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour {

    private float[] fruitDiameter = {0.25f,0.5f,0.75f,1.0f,1.25f,1.5f,1.75f,2.0f,2.25f,2.5f};
    public int fruitLevel = 1; // 1~10
    private Rigidbody2D rb;
    private GameObject picker;
    private GM gm;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        picker = GameObject.Find("FruitSpawner");
        gm = FindObjectOfType<GM>();

        fruitLevel = gm.fruitPreview[0]; // 현재 과일 레벨 설정
        gm.updatePreview = true; // 다음 과일로 변경 신호
    }

    //private void Awake() {
    //    transform.position = picker.transform.position;

    //}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Fruit_SuBak-Game")) return;

        gm.mergeSig = true;

        float mergeX = (transform.position.x + collision.transform.position.x) / 2;
        float mergeY = (transform.position.y + collision.transform.position.y) / 2;
        gm.mergePos = new Vector2(mergeX, mergeY);
    }
}

