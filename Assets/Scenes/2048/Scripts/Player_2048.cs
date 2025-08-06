using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2048 : MonoBehaviour {
    // 블록 프리펩 받기
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

    public GameObject[,] blockMap = new GameObject[4, 4]; // 해당 위치에 존재하는 블록 프리펩 저장
    public int[,] blockValue = new int[4, 4]; // 해당 위치에 존재하는 블록 값 저장


    public Vector2[,] posCode = new Vector2[4,4]; // 블록 위치 코드화
    public float[] x = { -1.8f, -0.6f, 0.6f, 1.8f }; // x위치코드
    public float[] y = { 0.3f, -0.9f, -2.1f, -3.3f }; // y위치코드


    private void Start() {
        // 블록 위치 코드 초기화
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                posCode[i,j] = new Vector2(x[i], y[j]);
                blockValue[i, j] = 0; // 블록 값 초기화
            }
        }
    }

    private void Update() {

    }

    public void CreateBlock() {
        // 2 또는 4 블록 생성
        int blockType = Random.Range(0, 10);
        GameObject blockPrefab = null;
        if (blockType < 9) {
            blockPrefab = block2; // 90% 확률로 2 블록 생성
            blockType = 2; // 블록 값 2로 설정
        } else {
            blockPrefab = block4; // 10% 확률로 4 블록 생성
            blockType = 4; // 블록 값 4로 설정
        }

        // 빈 칸에 블록 생성
        do {
            int i = Random.Range(0, 4);
            int j = Random.Range(0, 4);
            if (blockMap[i, j] == null) { // 빈 칸에만 생성
                GameObject newBlock = Instantiate(blockPrefab, new Vector2(i, j), Quaternion.identity);
                blockMap[i, j] = newBlock;
                blockValue[i, j] = blockType; // 블록 값 설정
                break; // 블록을 생성한 후 반복문 종료
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
        //합치기 단계
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 3; j++) {
                if (blockValue[i, j] == blockValue[i, j + 1] && blockValue[i, j] != 0) { // 블록과 왼쪽 블록 값 비교
                    // 블록 제거
                    blockMap[i, j].GetComponent<Block_2048>().Die();
                    blockMap[i, j + 1].GetComponent<Block_2048>().Die();

                    // 변수 업데이트
                    blockMap[i, j] = null; blockMap[i, j + 1] = null; // 현재 블록 제거
                    blockValue[i, j] *= 2; blockValue[i, j + 1] = 0; // 블록 값 업데이트

                    // 병합된 블록 생성
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
