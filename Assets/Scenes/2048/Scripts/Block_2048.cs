using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_2048 : MonoBehaviour {
    public Player_2048 pCode; // 플레이어 코드

    private int Xidx; // 블록 위치 코드 인덱스
    private int Yidx;
    private int myValue; // 블록 값

    private float speed = 0f; // 이동 속도

    bool moveY = false; // Y축 이동 여부
    bool moveX = false; // X축 이동 여부


    // Start is called before the first frame update
    void Start() {
        if (pCode == null) {
            pCode = FindObjectOfType<Player_2048>();
        }

        for (int i = 0; i < 4; i++) { // 블록 위치 코드 리스트에서
            for (int j = 0; j < 4; j++) {
                if (Vector3.Distance(pCode.posCode[i*4+j],transform.position) < 0.01f) { // 이동 시작 위치와 일치하는지 확인
                    Xidx = j;
                    Yidx = i; // 인덱스 저장
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (pCode.commanding == true) {
            Move(pCode.posCode[pCode.arr_des[Xidx,Yidx, 0]], pCode.posCode[pCode.arr_des[Xidx,Yidx, 1]], pCode.moveDirection); // 이동 함수 호출
        }

        if (pCode.isMoving[Xidx,Yidx] == true) { // 이동 중이라면
            if (moveY == true) { // Y축 이동이면
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, pCode.posCode[pCode.arr_des[Xidx,Yidx, 1]]) < 0.01f) {
                    pCode.isMoving[Xidx,Yidx] = false; // 이동 완료 후 이동 중 상태 해제
                }
            }
            else if (moveX == true) { // X축 이동이면
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, pCode.posCode[pCode.arr_des[Xidx, Yidx, 1]]) < 0.01f) {
                    pCode.isMoving[Xidx, Yidx] = false; // 이동 완료 후 이동 중 상태 해제
                }
            }
        }
    }


    // 이동 속도 설정 함수
    public void Move(Vector3 from, Vector3 to, string direction) {
        if (pCode.isMoving[Xidx,Yidx] == true) { // 이동 중이라면 리턴
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
        Destroy(gameObject); // 블록 제거
    }
}
