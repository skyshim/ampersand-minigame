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
    public float[] fruitDiameter = { 0.25f, 0.5f, 0.75f, 1.0f, 1.25f, 1.5f, 1.75f, 2.0f, 2.25f, 2.5f };

    // Start is called before the first frame update
    void Start() {
        mergeLevel = SpawnLevel();
    }

    // Update is called once per frame
    void Update() {

        if (mergeSig) {
            spawnType = true; // 병합으로 생성
            Instantiate(fruitprefab, mergePos, Quaternion.identity);
            mergeSig = false;
        }
    }

    int SpawnLevel() {
        int r = Random.Range(1, 7);
        if (r <= 3) return 1;
        else if (r <= 5) return 2;
        else return 3;
    }
}
