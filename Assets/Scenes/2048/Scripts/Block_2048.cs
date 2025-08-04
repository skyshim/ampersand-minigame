using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_2048 : MonoBehaviour
{
    public Player_2048 pCode; // 플레이어 코드

    private int idx; // 블록 위치 코드 인덱스
    private int myValue; // 블록 값

    float MoveTime = 0.1f; // 이동 시간 (초 단위)
    float speed = 0f; // 이동 속도

    bool moveY = false; // Y축 이동 여부
    bool moveX = false; // X축 이동 여부


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pCode.posCode.Length; i++) { // 블록 위치 코드 리스트에서
            if (pCode.posCode[i] == transform.position) { // 이동 시작 위치와 일치하는지 확인
                idx = i; // 인덱스 저장
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pCode.isMoving[idx] == true) { // 이동 중이라면
            if (moveY == true) { // Y축 이동이면
                MoveY(pCode.posCode[idx]); // Y축 이동 함수 호출
            }
            else if (moveX == true) { // X축 이동이면
                MoveX(pCode.posCode[idx]); // X축 이동 함수 호출
            }
        }
    }


    // 이동 속도 설정 함수
    public void Move(Vector3 from, Vector3 to, string direction) 
    {
        if (pCode.isMoving[idx] == true) { // 이동 중이라면 리턴
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

    // Y축 이동 함수
    void MoveY(Vector3 destination) 
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (transform.position == destination) {
            pCode.isMoving[idx] = false; // 이동 완료 후 이동 중 상태 해제
        }
    }
    // X축 이동 함수
    void MoveX(Vector3 destination) 
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position == destination) {
            pCode.isMoving[idx] = false; // 이동 완료 후 이동 중 상태 해제
        }
    }
}
