using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_1945 : MonoBehaviour
{
    public int bpm = 100;
    public GameObject enemy_gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            spawnGunner();
        }

    }

    void spawnGunner() {
        // 출발 지점 설정
        Vector2 spawnPos = new Vector2(Random.Range(-4f, 4f), Random.Range(0f, 6f));
        int fixValue = Random.Range(0, 3);
        switch (fixValue) {
            case 0:
                spawnPos.y = 6f;    
                break;
            case 1:
                spawnPos.x = -4f;
                break;
            case 2:
                spawnPos.x = 4f;
                break;
        }


        // 도착 지점 설정
        float x = Random.Range(-2f, 2f);
        int s = 1;
        if (Random.Range(0, 2) == 0) s = -1;
        float y = Mathf.Sqrt(4 - x * x);

        float m = (s * y - spawnPos.y) / (x - spawnPos.x); // 기울기
        float rad = Mathf.Atan(m); // 라디안
        float degree = spawnPos.x > 0 ? rad * Mathf.Rad2Deg +90 : (rad * Mathf.Rad2Deg - 90f); // 각도

        Instantiate(enemy_gun, spawnPos, Quaternion.Euler(0, 0, degree));
    }
}
