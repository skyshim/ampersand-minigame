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

    public GameObject[,] blockMap = new GameObject[4, 4];

    public Vector3[] posCode = new Vector3[16]; // 블록 위치 코드화
    public float[] x = { -1.8f, -0.6f, 0.6f, 1.8f }; // x위치코드
    public float[] y = { 0.3f, -0.9f, -2.1f, -3.3f }; // y위치코드
    public int[,] blockValue = new int[4, 4]; // 블록 값 리스트
    public bool[,] isMoving = new bool[4, 4]; // 블록 이동 중인지 여부 리스트
    public int[,,] arr_des = new int[4, 4, 2]; // 블록 이동 위치 배열
    public string moveDirection = "";

    public bool playing = false;
    public bool commanding = false;
    public bool merged = false;


    // Start is called before the first frame update
    void Start() {
        // 블록 위치 코드 설정 및 리스트 초기화
        int idx = 0;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                posCode[idx] = new Vector3(x[i], y[j], -2f); // 블록 위치 코드 설정
                blockValue[i, j] = 0; // 블록 값 초기화
                isMoving[i, j] = false; // 블록 이동 중 여부 초기화
                for (int k = 0; k < 2; k++) {
                    arr_des[i, j, k] = 0; // 블록 이동 위치 배열 초기화
                }
                idx++;
            }
        }
    }

    // Update is called once per frame
    void Update() {

        if (Input.anyKeyDown && (playing == false)) { // 게임 시작   
            GameStart();
        }

        // 입력받기
        bool Linput = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        bool Rinput = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool Uinput = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool Dinput = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

        if (Linput) { // 왼쪽 이동
            moveDirection = "A"; // 왼쪽 이동 방향 설정
            commanding = true; // 명령 중 상태 설정
            merged = false;
        }
        else if (Rinput) { // 오른쪽 이동
            moveDirection = "D"; // 오른쪽 이동 방향 설정
            commanding = true;
            merged = false; // 병합 여부 초기화
        }
        else if (Uinput) { // 위쪽 이동
            moveDirection = "W"; // 위쪽 이동 방향 설정
            commanding = true; // 명령 중 상태 설정
            merged = false;
        }
        else if (Dinput) { // 아래쪽 이동
            moveDirection = "S"; // 아래쪽 이동 방향 설정
            commanding = true; // 명령 중 상태 설정
            merged = false;
        }

        if (commanding == false) return; // 명령 중이 아니면 리턴
        if (merged == false) MergeProcess(moveDirection); // 블록 이동 전 병합 처리
        MoveCommand(moveDirection); // 블록 이동 명령 처리
        CreateBlock(); // 블록 생성
    }


    // 게임 시작
    void GameStart() {
        CreateBlock();
        CreateBlock(); // 블록 2개 생성
        playing = true;

    }


    // 블록 생성 함수
    public void CreateBlock() {
        int XspawnPosCode; // 블록 생성 위치 코드
        int YspawnPosCode;

        // 블록 생성 위치 탐색
        int idx = 0;
        while (true) {
            XspawnPosCode = Random.Range(0, 4); // 0~3 사이의 랜덤 코드 생성
            YspawnPosCode = Random.Range(0, 4);

            if (blockValue[XspawnPosCode, YspawnPosCode] == 0) { // 블록이 없는 위치를 찾음
                break; // 블록 생성 위치 탐색 종료
            }
            if (idx > 17) { // 블록 생성 위치 탐색 실패
                Debug.Log("블록 생성 위치 탐색 실패");
                return; // 블록 생성 위치 탐색 실패시 함수 종료
            }
            idx++;
        }
        Vector3 spawnPos = posCode[YspawnPosCode * 4 + XspawnPosCode];

        // 블록 생성
        int spawnWhat = Random.Range(0, 2);
        if (spawnWhat == 0) { // 2블록 생성
            blockMap[XspawnPosCode, YspawnPosCode] = Instantiate(block2, spawnPos, Quaternion.identity);
            blockValue[XspawnPosCode, YspawnPosCode] = 2; // 블록 값 설정
        }
        else { // 4블록 생성
            blockMap[XspawnPosCode, YspawnPosCode] = Instantiate(block4, spawnPos, Quaternion.identity);
            blockValue[XspawnPosCode, YspawnPosCode] = 4; // 블록 값 설정
        }

        if (commanding == true) { // 명령 중 상태라면
            commanding = false; // 명령 중 상태 해제
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


    // 블록 이동 전 병합 처리 함수
    public void MergeProcess(string direction) {
        if (direction == "A") { // 왼쪽 이동 시 병합
            for (int i = 0; i < 4; i++) { // 각 행에 대해
                for (int j = 0; j < 3; j++) { // 각 열에 대해
                    if (blockValue[j, i] == blockValue[j + 1, i] && blockValue[j, i] != 0) {
                        blockMap[j, i].GetComponent<Block_2048>().Die();
                        blockMap[j + 1, i].GetComponent<Block_2048>().Die(); // 병합할 블록 제거

                        blockValue[i, j] *= 2; // 병합된 블록 값 설정
                        CreateMergedBlock(posCode[i * 4 + j], blockValue[j,i]); // 병합된 블록 생성
                        blockValue[j+1,i] = 0; // 병합된 블록 제거
                    }
                }
            }
        }
        else if (direction == "D") { // 오른쪽 이동 시 병합
            for (int i = 0; i < 4; i++) { // 각 행에 대해
                for (int j = 3; j > 0; j--) { // 각 열에 대해
                    if (blockValue[j,i] == blockValue[j-1,i] && blockValue[j,i] != 0) {
                        blockMap[j, i].GetComponent<Block_2048>().Die();
                        blockMap[j - 1, i].GetComponent<Block_2048>().Die(); // 병합할 블록 제거

                        blockValue[j,i] *= 2; // 병합된 블록 값 설정
                        CreateMergedBlock(posCode[i * 4 + j], blockValue[j,i]); // 병합된 블록 생성
                        blockValue[j-1,i] = 0; // 병합된 블록 제거
                    }
                }
            }
        }
        else if (direction == "W") { // 위쪽 이동 시 병합
            for (int j = 0; j < 4; j++) { // 각 열에 대해
                for (int i = 0; i < 3; i++) { // 각 행에 대해
                    if (blockValue[j,i] == blockValue[j,i+1] && blockValue[j,i] != 0) {
                        blockMap[j, i].GetComponent<Block_2048>().Die();
                        blockMap[j, i + 1].GetComponent<Block_2048>().Die(); // 병합할 블록 제거

                        blockValue[j,i] *= 2; // 병합된 블록 값 설정
                        CreateMergedBlock(posCode[i * 4 + j], blockValue[j,i]); // 병합된 블록 생성
                        blockValue[j,i+1] = 0; // 병합된 블록 제거
                    }
                }
            }
        }
        else if (direction == "S") { // 아래쪽 이동 시 병합
            for (int j = 0; j < 4; j++) { // 각 열에 대해
                for (int i = 3; i > 0; i--) { // 각 행에 대해
                    if (blockValue[j,i] == blockValue[j,i-1] && blockValue[j,i] != 0) {
                        blockMap[j, i].GetComponent<Block_2048>().Die();
                        blockMap[j, i - 1].GetComponent<Block_2048>().Die(); // 병합할 블록 제거

                        blockValue[j,i] *= 2; // 병합된 블록 값 설정
                        CreateMergedBlock(posCode[i * 4 + j], blockValue[j,i]); // 병합된 블록 생성
                        blockValue[j,i-1] = 0; // 병합된 블록 제거
                    }
                }
            }
        }

        merged = true; // 병합 완료 상태 설정
    }


    // 블록 이동 명령 함수
    public void MoveCommand(string direction) {
        if (direction == "A") {
            for (int y = 0; y < 4; y++) {

            }
        }
    }
}
