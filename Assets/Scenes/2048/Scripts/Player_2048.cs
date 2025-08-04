using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2048 : MonoBehaviour
{
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


    public Vector3[] posCode = new Vector3[16]; // 블록 위치 코드화
    public float[] x = { -1.8f, -0.6f, 0.6f, 1.8f }; // x위치코드
    public float[] y = { 0.3f, -0.9f, -2.1f, -3.3f }; // y위치코드
    public int[] blockValue = new int[16]; // 블록 값 리스트
    public bool[] isMoving = new bool[16]; // 블록 이동 중인지 여부 리스트
    public int[,] arr_des = new int[16,2]; // 블록 이동 위치 배열

    public bool playing = false;
    // Start is called before the first frame update
    void Start() 
    {
        // 블록 위치 코드 설정
        int idx = 0;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                posCode[idx] = new Vector3(x[i], y[j], -2f); // 블록 위치 코드 설정
                idx++;
            }
        }

        // 블록 리스트 초기화
        for (int i = 0; i < 16; i++) {
            blockValue[i] = 0; // 블록 값 초기화
        }
        for (int i = 0; i < 16; i++) {
            for (int j = 0; j < 2; j++) {
                arr_des[i,j] = 0; // 블록 이동 위치 배열 초기화
            }
        }



    }

    // Update is called once per frame
    void Update() 
    {
        
        if (Input.anyKeyDown && (playing == false)) { // 게임 시작   
            GameStart();
        }

        string direction = ""; // 이동 방향 변수 초기화

        // 입력받기
        bool Linput = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow); 
        bool Rinput = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool Uinput = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow); 
        bool Dinput = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        
        if (Linput) { // 왼쪽 이동
            direction = "A"; // 왼쪽 이동 방향 설정
        } else if (Rinput) { // 오른쪽 이동
            direction = "D"; // 오른쪽 이동 방향 설정
        } else if (Uinput) { // 위쪽 이동
            direction = "W"; // 위쪽 이동 방향 설정
        } else if (Dinput) { // 아래쪽 이동
            direction = "S"; // 아래쪽 이동 방향 설정
        }

        MergeProcess(direction); // 블록 이동 전 병합 처리

    }


    // 게임 시작
    void GameStart() {
        CreateBlock();
        CreateBlock(); // 블록 2개 생성
        playing = true;

    }


    // 블록 생성 함수
    public void CreateBlock() 
    {
        int spawnPosCode; // 블록 생성 위치 코드
        // 블록 생성 위치 탐색
        int idx = 0;
        while(true) {
            spawnPosCode = Random.Range(0, 16); // 0~15 사이의 랜덤 코드 생성
            if (blockValue[spawnPosCode] == 0) { // 블록이 없는 위치를 찾음
                break; // 블록 생성 위치 탐색 종료
            }
            if (idx > 100) { // 블록 생성 위치 탐색 실패
                Debug.Log("블록 생성 위치 탐색 실패");
                return; // 블록 생성 위치 탐색 실패시 함수 종료
            }
            idx++;
        }
        Vector3 spawnPos = posCode[spawnPosCode];

        // 블록 생성
        int spawnWhat = Random.Range(0, 2);
        if (spawnWhat == 0) { // 2블록 생성
            Instantiate(block2, spawnPos, Quaternion.identity);
            blockValue[spawnPosCode] = 2; // 블록 값 설정
        } else { // 4블록 생성
            Instantiate(block4, spawnPos, Quaternion.identity);
            blockValue[spawnPosCode] = 4; // 블록 값 설정
        }
    }


    public void CreateMergedBlock(Vector3 spawnPos, int spawnNum) {
        if (spawnNum == 4) {
            Instantiate(block4, spawnPos, Quaternion.identity);
        } else if (spawnNum == 8) {
            Instantiate(block8, spawnPos, Quaternion.identity);
        } else if (spawnNum == 16) {
            Instantiate(block16, spawnPos, Quaternion.identity);
        } else if (spawnNum == 32) {
            Instantiate(block32, spawnPos, Quaternion.identity);
        } else if (spawnNum == 64) {
            Instantiate(block64, spawnPos, Quaternion.identity);
        } else if (spawnNum == 128) {
            Instantiate(block128, spawnPos, Quaternion.identity);
        } else if (spawnNum == 256) {
            Instantiate(block256, spawnPos, Quaternion.identity);
        } else if (spawnNum == 512) {
            Instantiate(block512, spawnPos, Quaternion.identity);
        } else if (spawnNum == 1024) {
            Instantiate(block1024, spawnPos, Quaternion.identity);
        } else if (spawnNum == 2048) {
            Instantiate(block2048, spawnPos, Quaternion.identity);
        }
    }


    // 블록 이동 전 병합 처리 함수
    public void MergeProcess(string direction) {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                if (direction == "A") { // 왼쪽 이동 시 병합
                    if (blockValue[i * 4 + j] == blockValue[i * 4 + j + 1] && blockValue[i * 4 + j] != 0) {
                        blockValue[i * 4 + j + 1] *= 2; // 병합된 블록 값 설정
                        CreateMergedBlock(posCode[i * 4 + j], blockValue[i * 4 + j]); // 병합된 블록 생성
                        blockValue[i * 4 + j + 1] = 0; // 병합된 블록 제거
                    }
                }
                else if (direction == "D") { // 오른쪽 이동 시 병합
                    if (blockValue[i * 4 + 4 - j] == blockValue[i * 4 + 3 - j] && blockValue[i * 4 + 4 - j] != 0) {
                        blockValue[(i + 1) * 4 + 4 - j] *= 2;
                        CreateMergedBlock(posCode[i * 4 + 4 - j], blockValue[i * 4 + 4 - j]);
                        blockValue[(i + 1) * 4 + 3 - j] = 0;
                    }
                }
                else if (direction == "W") { // 위쪽 이동 시 병합
                    if (blockValue[(4 - i) * 4 + j] == blockValue[(3 - i) * 4 + j] && blockValue[i * 4 + j] != 0) {
                        blockValue[(4 - i) * 4 + j] *= 2;
                        CreateMergedBlock(posCode[(4 - i) * 4 + j], blockValue[(4 - i) * 4 + j]);
                        blockValue[(3 - i) * 4 + j] = 0;
                    }
                }
                else if (direction == "S") { // 아래쪽 이동 시 병합
                    if (blockValue[i * 4 + j] == blockValue[(i + 1) * 4 + j] && blockValue[i * 4 + j] != 0) {
                        blockValue[i * 4 + j] *= 2;
                        CreateMergedBlock(posCode[i * 4 + j], blockValue[i * 4 + j]);
                        blockValue[(i + 1) * 4 + j] = 0;
                    }
                }
            }
        }
    }


    // 블록 이동 명령 함수
    public void MoveCommand(string direction) {
        if (direction == "D") {

        }
    }
}
