using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_2048 : MonoBehaviour
{
    public Player_2048 pCode; // �÷��̾� �ڵ�

    private int idx; // ��� ��ġ �ڵ� �ε���
    private int myValue; // ��� ��

    float MoveTime = 0.1f; // �̵� �ð� (�� ����)
    float speed = 0f; // �̵� �ӵ�

    bool moveY = false; // Y�� �̵� ����
    bool moveX = false; // X�� �̵� ����


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pCode.posCode.Length; i++) { // ��� ��ġ �ڵ� ����Ʈ����
            if (pCode.posCode[i] == transform.position) { // �̵� ���� ��ġ�� ��ġ�ϴ��� Ȯ��
                idx = i; // �ε��� ����
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pCode.isMoving[idx] == true) { // �̵� ���̶��
            if (moveY == true) { // Y�� �̵��̸�
                MoveY(pCode.posCode[idx]); // Y�� �̵� �Լ� ȣ��
            }
            else if (moveX == true) { // X�� �̵��̸�
                MoveX(pCode.posCode[idx]); // X�� �̵� �Լ� ȣ��
            }
        }
    }


    // �̵� �ӵ� ���� �Լ�
    public void Move(Vector3 from, Vector3 to, string direction) 
    {
        if (pCode.isMoving[idx] == true) { // �̵� ���̶�� ����
            return;
        }
        else {
            pCode.isMoving[idx] = true;
        }

        if (direction == "Y") {
            speed = to.y - from.y;
            moveY = true;
        }
        else if (direction == "X") {
            speed = to.x - from.x;
            moveX = true;
        }
    }

    // Y�� �̵� �Լ�
    void MoveY(Vector3 destination) 
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (transform.position == destination) {
            pCode.isMoving[idx] = false; // �̵� �Ϸ� �� �̵� �� ���� ����
        }
    }
    // X�� �̵� �Լ�
    void MoveX(Vector3 destination) 
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position == destination) {
            pCode.isMoving[idx] = false; // �̵� �Ϸ� �� �̵� �� ���� ����
        }
    }
}
