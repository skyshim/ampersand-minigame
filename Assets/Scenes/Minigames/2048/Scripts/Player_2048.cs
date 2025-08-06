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

    public GameObject[,] blockMap = new GameObject[4, 4]; // �ش� ��ġ�� �����ϴ� ��� ������ ����
    public int[,] blockValue = new int[4, 4]; // �ش� ��ġ�� �����ϴ� ��� �� ����


    public Vector2[,] posCode = new Vector2[4,4]; // ��� ��ġ �ڵ�ȭ
    public float[] x = { -1.8f, -0.6f, 0.6f, 1.8f }; // x��ġ�ڵ�
    public float[] y = { 0.3f, -0.9f, -2.1f, -3.3f }; // y��ġ�ڵ�


    private void Start() {
        // ��� ��ġ �ڵ� �ʱ�ȭ
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                posCode[i,j] = new Vector2(x[i], y[j]);
                blockValue[i, j] = 0; // ��� �� �ʱ�ȭ
            }
        }
    }

    private void Update() {

    }

    public void CreateBlock() {
        // 2 �Ǵ� 4 ��� ����
        int blockType = Random.Range(0, 10);
        GameObject blockPrefab = null;
        if (blockType < 9) {
            blockPrefab = block2; // 90% Ȯ���� 2 ��� ����
            blockType = 2; // ��� �� 2�� ����
        } else {
            blockPrefab = block4; // 10% Ȯ���� 4 ��� ����
            blockType = 4; // ��� �� 4�� ����
        }

        // �� ĭ�� ��� ����
        do {
            int i = Random.Range(0, 4);
            int j = Random.Range(0, 4);
            if (blockMap[i, j] == null) { // �� ĭ���� ����
                GameObject newBlock = Instantiate(blockPrefab, new Vector2(i, j), Quaternion.identity);
                blockMap[i, j] = newBlock;
                blockValue[i, j] = blockType; // ��� �� ����
                break; // ����� ������ �� �ݺ��� ����
            }
        } while (true);
    }


    private void CommandLeft() {
        bool isMoving = true;
        for (int y = 0; y < 4; y++) {
            for (int x = 1; x < 4; x++) {
                if (blockMap[x, y] != null) {
                    int targetX = x;
                    while (targetX > 0 && blockMap[targetX - 1, y] == null) {
                        targetX--;
                    }

                    if (targetX != x) {
                        blockMap[targetX, y] = blockMap[x, y];
                        blockValue[targetX, y] = blockValue[x, y];

                        blockMap[x, y] = null;
                        blockValue[x, y] = 0;

                        blockMap[targetX, y].GetComponent<Block_2048>().Move(posCode[x, y], posCode[targetX, y]);
                    }
                }
            }
        }

        if (isMoving) return;
        //��ġ�� �ܰ�
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 3; j++) {
                if (blockValue[i, j] == blockValue[i, j + 1] && blockValue[i, j] != 0) { // ��ϰ� ���� ��� �� ��
                    // ��� ����
                    blockMap[i, j].GetComponent<Block_2048>().Die();
                    blockMap[i, j + 1].GetComponent<Block_2048>().Die();

                    // ���� ������Ʈ
                    blockMap[i, j] = null; blockMap[i, j + 1] = null; // ���� ��� ����
                    blockValue[i, j] *= 2; blockValue[i, j + 1] = 0; // ��� �� ������Ʈ

                    // ���յ� ��� ����
                    GameObject blockPrefab = null;
                    switch (blockValue[i, j]) {
                        case 4: blockPrefab = block4; break;
                        case 8: blockPrefab = block8; break;
                        case 16: blockPrefab = block16; break;
                        case 32: blockPrefab = block32; break;
                        case 64: blockPrefab = block64; break;
                        case 128: blockPrefab = block128; break;
                        case 256: blockPrefab = block256; break;
                        case 512: blockPrefab = block512; break;
                        case 1024: blockPrefab = block1024; break;
                        case 2048: blockPrefab = block2048; break;
                    }
                    GameObject newBlock = Instantiate(blockPrefab, new Vector2(i, j), Quaternion.identity);
                    blockMap[i, j] = newBlock;
                }
            }
        }
    }

}
