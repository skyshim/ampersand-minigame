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


    private Vector3[] posCode = new Vector3[16]; // 블록 위치 코드 리스트
    private float[] x = { -1.8f, -0.6f, 0.6f, 1.8f }; // x위치코드
    private float[] y = { 0.3f, -0.9f, -2.1f, -3.3f }; // y위치코드
    List<bool> isBlock = new List<bool>(); // 블록 리스트
    private int[] blockValue = new int[16];

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
            isBlock.Add(false); // 블록이 있는지 없는지 초기화
            blockValue[i] = 0; // 블록 값 초기화
        }


    }

    // Update is called once per frame
    void Update() 
    {
        
        if (Input.anyKeyDown && (playing == false)) { // 게임 시작   
            GameStart();
        }

        // 입력받기
        bool Linput = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow); 
        bool Rinput = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool Uinput = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow); 
        bool Dinput = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);



    }


    // 게임 시작 실행
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
            if (isBlock[spawnPosCode] == false) { // 블록이 없는 위치를 찾음
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
        isBlock[spawnPosCode] = true; // 블록 존재함 알림 true
    }
}
