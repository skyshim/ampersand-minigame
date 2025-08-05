using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_2048 : MonoBehaviour {
    public Player_2048 pCode; // �÷��̾� �ڵ�

    private int Xidx; // ��� ��ġ �ڵ� �ε���
    private int Yidx;
    private int myValue; // ��� ��

    private float speed = 0f; // �̵� �ӵ�

    bool moveY = false; // Y�� �̵� ����
    bool moveX = false; // X�� �̵� ����


    // Start is called before the first frame update
    void Start() {
        if (pCode == null) {
            pCode = FindObjectOfType<Player_2048>();
        }

        for (int i = 0; i < 4; i++) { // ��� ��ġ �ڵ� ����Ʈ����
            for (int j = 0; j < 4; j++) {
                if (Vector3.Distance(pCode.posCode[i*4+j],transform.position) < 0.01f) { // �̵� ���� ��ġ�� ��ġ�ϴ��� Ȯ��
                    Xidx = j;
                    Yidx = i; // �ε��� ����
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (pCode.commanding == true) {
            Move(pCode.posCode[pCode.arr_des[Xidx,Yidx, 0]], pCode.posCode[pCode.arr_des[Xidx,Yidx, 1]], pCode.moveDirection); // �̵� �Լ� ȣ��
        }

        if (pCode.isMoving[Xidx,Yidx] == true) { // �̵� ���̶��
            if (moveY == true) { // Y�� �̵��̸�
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, pCode.posCode[pCode.arr_des[Xidx,Yidx, 1]]) < 0.01f) {
                    pCode.isMoving[Xidx,Yidx] = false; // �̵� �Ϸ� �� �̵� �� ���� ����
                }
            }
            else if (moveX == true) { // X�� �̵��̸�
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, pCode.posCode[pCode.arr_des[Xidx, Yidx, 1]]) < 0.01f) {
                    pCode.isMoving[Xidx, Yidx] = false; // �̵� �Ϸ� �� �̵� �� ���� ����
                }
            }
        }
    }


    // �̵� �ӵ� ���� �Լ�
    public void Move(Vector3 from, Vector3 to, string direction) {
        if (pCode.isMoving[Xidx,Yidx] == true) { // �̵� ���̶�� ����
            return;
        }
        else {
            pCode.isMoving[Xidx,Yidx] = true;
        }

        if (direction == "W" || direction == "S") {
            speed = to.y - from.y;
            moveY = true;
        }
        else if (direction == "A" || direction == "D") {
            speed = to.x - from.x;
            moveX = true;
        }
    }

    public void Die() {
        Destroy(gameObject); // ��� ����
    }
}
