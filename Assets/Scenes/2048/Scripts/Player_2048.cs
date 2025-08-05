using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2048 : MonoBehaviour {
    // ��� ������ �ޱ�
    public GameObject block2;
    public GameObject block4;
    public GameObject block8;
    public GameObject block16;
    public GameObject block32;
    public GameObject block64;
    public GameObject block128;
    public GameObject block256;
    public GameObject block512;
    public GameObject block1024;
    public GameObject block2048;

    public GameObject[,] blockMap = new GameObject[4, 4];

    public Vector3[] posCode = new Vector3[16]; // ��� ��ġ �ڵ�ȭ
    public float[] x = { -1.8f, -0.6f, 0.6f, 1.8f }; // x��ġ�ڵ�
    public float[] y = { 0.3f, -0.9f, -2.1f, -3.3f }; // y��ġ�ڵ�
    public int[,] blockValue = new int[4, 4]; // ��� �� ����Ʈ
    public bool[,] isMoving = new bool[4, 4]; // ��� �̵� ������ ���� ����Ʈ
    public int[,,] arr_des = new int[4, 4, 2]; // ��� �̵� ��ġ �迭
    public string moveDirection = "";

    public bool playing = false;
    public bool commanding = false;
    public bool merged = false;


    // Start is called before the first frame update
    void Start() {
        // ��� ��ġ �ڵ� ���� �� ����Ʈ �ʱ�ȭ
        int idx = 0;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                posCode[idx] = new Vector3(x[i], y[j], -2f); // ��� ��ġ �ڵ� ����
                blockValue[i, j] = 0; // ��� �� �ʱ�ȭ
                isMoving[i, j] = false; // ��� �̵� �� ���� �ʱ�ȭ
                for (int k = 0; k < 2; k++) {
                    arr_des[i, j, k] = 0; // ��� �̵� ��ġ �迭 �ʱ�ȭ
                }
                idx++;
            }
        }
    }

    // Update is called once per frame
    void Update() {

        if (Input.anyKeyDown && (playing == false)) { // ���� ����   
            GameStart();
        }

        // �Է¹ޱ�
        bool Linput = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        bool Rinput = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool Uinput = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool Dinput = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

        if (Linput) { // ���� �̵�
            moveDirection = "A"; // ���� �̵� ���� ����
            commanding = true; // ��� �� ���� ����
            merged = false;
        }
        else if (Rinput) { // ������ �̵�
            moveDirection = "D"; // ������ �̵� ���� ����
            commanding = true;
            merged = false; // ���� ���� �ʱ�ȭ
        }
        else if (Uinput) { // ���� �̵�
            moveDirection = "W"; // ���� �̵� ���� ����
            commanding = true; // ��� �� ���� ����
            merged = false;
        }
        else if (Dinput) { // �Ʒ��� �̵�
            moveDirection = "S"; // �Ʒ��� �̵� ���� ����
            commanding = true; // ��� �� ���� ����
            merged = false;
        }

        if (commanding == false) return; // ��� ���� �ƴϸ� ����
        if (merged == false) MergeProcess(moveDirection); // ��� �̵� �� ���� ó��
        MoveCommand(moveDirection); // ��� �̵� ��� ó��
        CreateBlock(); // ��� ����
    }


    // ���� ����
    void GameStart() {
        CreateBlock();
        CreateBlock(); // ��� 2�� ����
        playing = true;

    }


    // ��� ���� �Լ�
    public void CreateBlock() {
        int XspawnPosCode; // ��� ���� ��ġ �ڵ�
        int YspawnPosCode;

        // ��� ���� ��ġ Ž��
        int idx = 0;
        while (true) {
            XspawnPosCode = Random.Range(0, 4); // 0~3 ������ ���� �ڵ� ����
            YspawnPosCode = Random.Range(0, 4);

            if (blockValue[XspawnPosCode, YspawnPosCode] == 0) { // ����� ���� ��ġ�� ã��
                break; // ��� ���� ��ġ Ž�� ����
            }
            if (idx > 17) { // ��� ���� ��ġ Ž�� ����
                Debug.Log("��� ���� ��ġ Ž�� ����");
                return; // ��� ���� ��ġ Ž�� ���н� �Լ� ����
            }
            idx++;
        }
        Vector3 spawnPos = posCode[YspawnPosCode * 4 + XspawnPosCode];

        // ��� ����
        int spawnWhat = Random.Range(0, 2);
        if (spawnWhat == 0) { // 2��� ����
            blockMap[XspawnPosCode, YspawnPosCode] = Instantiate(block2, spawnPos, Quaternion.identity);
            blockValue[XspawnPosCode, YspawnPosCode] = 2; // ��� �� ����
        }
        else { // 4��� ����
            blockMap[XspawnPosCode, YspawnPosCode] = Instantiate(block4, spawnPos, Quaternion.identity);
            blockValue[XspawnPosCode, YspawnPosCode] = 4; // ��� �� ����
        }

        if (commanding == true) { // ��� �� ���¶��
            commanding = false; // ��� �� ���� ����
        }
    }


    public void CreateMergedBlock(Vector3 spawnPos, int spawnNum) {
        if (spawnNum == 4) {
            Instantiate(block4, spawnPos, Quaternion.identity);
        }
        else if (spawnNum == 8) {
            Instantiate(block8, spawnPos, Quaternion.identity);
        }
        else if (spawnNum == 16) {
            Instantiate(block16, spawnPos, Quaternion.identity);
        }
        else if (spawnNum == 32) {
            Instantiate(block32, spawnPos, Quaternion.identity);
        }
        else if (spawnNum == 64) {
            Instantiate(block64, spawnPos, Quaternion.identity);
        }
        else if (spawnNum == 128) {
            Instantiate(block128, spawnPos, Quaternion.identity);
        }
        else if (spawnNum == 256) {
            Instantiate(block256, spawnPos, Quaternion.identity);
        }
        else if (spawnNum == 512) {
            Instantiate(block512, spawnPos, Quaternion.identity);
        }
        else if (spawnNum == 1024) {
            Instantiate(block1024, spawnPos, Quaternion.identity);
        }
        else if (spawnNum == 2048) {
            Instantiate(block2048, spawnPos, Quaternion.identity);
        }
    }


    // ��� �̵� �� ���� ó�� �Լ�
    public void MergeProcess(string direction) {
        if (direction == "A") { // ���� �̵� �� ����
            for (int i = 0; i < 4; i++) { // �� �࿡ ����
                for (int j = 0; j < 3; j++) { // �� ���� ����
                    if (blockValue[j, i] == blockValue[j + 1, i] && blockValue[j, i] != 0) {
                        blockMap[j, i].GetComponent<Block_2048>().Die();
                        blockMap[j + 1, i].GetComponent<Block_2048>().Die(); // ������ ��� ����

                        blockValue[i, j] *= 2; // ���յ� ��� �� ����
                        CreateMergedBlock(posCode[i * 4 + j], blockValue[j,i]); // ���յ� ��� ����
                        blockValue[j+1,i] = 0; // ���յ� ��� ����
                    }
                }
            }
        }
        else if (direction == "D") { // ������ �̵� �� ����
            for (int i = 0; i < 4; i++) { // �� �࿡ ����
                for (int j = 3; j > 0; j--) { // �� ���� ����
                    if (blockValue[j,i] == blockValue[j-1,i] && blockValue[j,i] != 0) {
                        blockMap[j, i].GetComponent<Block_2048>().Die();
                        blockMap[j - 1, i].GetComponent<Block_2048>().Die(); // ������ ��� ����

                        blockValue[j,i] *= 2; // ���յ� ��� �� ����
                        CreateMergedBlock(posCode[i * 4 + j], blockValue[j,i]); // ���յ� ��� ����
                        blockValue[j-1,i] = 0; // ���յ� ��� ����
                    }
                }
            }
        }
        else if (direction == "W") { // ���� �̵� �� ����
            for (int j = 0; j < 4; j++) { // �� ���� ����
                for (int i = 0; i < 3; i++) { // �� �࿡ ����
                    if (blockValue[j,i] == blockValue[j,i+1] && blockValue[j,i] != 0) {
                        blockMap[j, i].GetComponent<Block_2048>().Die();
                        blockMap[j, i + 1].GetComponent<Block_2048>().Die(); // ������ ��� ����

                        blockValue[j,i] *= 2; // ���յ� ��� �� ����
                        CreateMergedBlock(posCode[i * 4 + j], blockValue[j,i]); // ���յ� ��� ����
                        blockValue[j,i+1] = 0; // ���յ� ��� ����
                    }
                }
            }
        }
        else if (direction == "S") { // �Ʒ��� �̵� �� ����
            for (int j = 0; j < 4; j++) { // �� ���� ����
                for (int i = 3; i > 0; i--) { // �� �࿡ ����
                    if (blockValue[j,i] == blockValue[j,i-1] && blockValue[j,i] != 0) {
                        blockMap[j, i].GetComponent<Block_2048>().Die();
                        blockMap[j, i - 1].GetComponent<Block_2048>().Die(); // ������ ��� ����

                        blockValue[j,i] *= 2; // ���յ� ��� �� ����
                        CreateMergedBlock(posCode[i * 4 + j], blockValue[j,i]); // ���յ� ��� ����
                        blockValue[j,i-1] = 0; // ���յ� ��� ����
                    }
                }
            }
        }

        merged = true; // ���� �Ϸ� ���� ����
    }


    // ��� �̵� ��� �Լ�
    public void MoveCommand(string direction) {
        if (direction == "A") {
            for (int y = 0; y < 4; y++) {

            }
        }
    }
}
