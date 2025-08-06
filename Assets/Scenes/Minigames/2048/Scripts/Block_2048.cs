using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_2048 : MonoBehaviour {
    public Player_2048 pCode; // 플레이어 코드


    // Start is called before the first frame update
    void Start() {
        if (pCode == null) {
            pCode = FindObjectOfType<Player_2048>();
        }

    }

    // 블록 제거 함수
    public void Die() {
        Destroy(gameObject);
    }


    // 이동 함수
    public void Move(Vector2 from, Vector2 to) {

    }
}
