using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
    private float go = 0;
    public int spawnLevel = 1; // 1~10
    private float formerclick;
    public float clickDelay = 0.1f; // 클릭 딜레이

    public GameObject fruitPrefab;
    public GameObject spawnPoint;
    private GM gm;

    // Start is called before the first frame update
    void Start() {
        gm = FindObjectOfType<GM>();
        spawnPoint.transform.localScale = new Vector3(gm.fruitDiameter[spawnLevel - 1], gm.fruitDiameter[spawnLevel - 1], 1);
        formerclick = Time.time;
    }

    // Update is called once per frame
    void Update() {
        // 이동 명령
        if (transform.position.x < -2.4) {
            go = Input.GetAxisRaw("Horizontal");
            if (go < 0) go = 0;
            transform.Translate(new Vector2(go, 0) * Time.deltaTime * 5);
        } else if (transform.position.x > 2.4) {
            go = Input.GetAxisRaw("Horizontal");
            if (go > 0) go = 0;
            transform.Translate(new Vector2(go, 0) * Time.deltaTime * 5);
        } else {
            go = Input.GetAxisRaw("Horizontal");
            transform.Translate(new Vector2(go, 0) * Time.deltaTime * 5);
        }


        if (Input.GetKeyDown(KeyCode.Space) && Time.time - formerclick > clickDelay) {
            formerclick = Time.time;
            int r = Random.Range(1, 7);
            if (r <= 3) spawnLevel = 1;
            else if (r <= 5) spawnLevel = 2;
            else spawnLevel = 3;

            spawnPoint.transform.localScale = new Vector3(gm.fruitDiameter[spawnLevel - 1], gm.fruitDiameter[spawnLevel - 1], 1);

            gm.spawnType = false; // 클릭으로 생성
            Instantiate(fruitPrefab, spawnPoint.transform.position, transform.rotation); // 이후 과일 생성
        }
    }
}

