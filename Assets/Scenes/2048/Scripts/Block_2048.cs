using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_2048 : MonoBehaviour {
    public Player_2048 pCode; // �÷��̾� �ڵ�


    // Start is called before the first frame update
    void Start() {
        if (pCode == null) {
            pCode = FindObjectOfType<Player_2048>();
        }

    }

    // ��� ���� �Լ�
    public void Die() {
        Destroy(gameObject);
    }


    // �̵� �Լ�
    public void Move(Vector2 from, Vector2 to) {

    }
}
