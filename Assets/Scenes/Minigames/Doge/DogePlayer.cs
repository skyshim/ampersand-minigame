using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DogePlayer : MonoBehaviour
{
    [Header("마우스 따라가기 속도")]
    public float moveSpeed = 10f;

    private Rigidbody2D rb;
    private bool isGameOver = false;

    private GameObject DogeOverField;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // DogeOverField 자동 검색
        DogeOverField = GameObject.Find("DogeOverField");
        if (DogeOverField != null)
            DogeOverField.SetActive(false); // 시작 시 비활성화
        else
            Debug.LogWarning("Scene에 DogeOverField 오브젝트가 없습니다!");
    }

    void FixedUpdate()
    {
        if (isGameOver)
        {
            rb.velocity = Vector2.zero; // 이동 정지
            return;
        }

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // 방향 계산 후 속도 적용
        Vector2 direction = (mousePos - transform.position);
        rb.velocity = direction * moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // DogeBlock1 계열과 충돌 시 게임 종료
        string[] blockNames = { "DogeBlock1", "DogeBlock1 (1)", "DogeBlock1 (2)", "DogeBlock1 (3)" };

        foreach (string name in blockNames)
        {
            if (collision.gameObject.name == name)
            {
                Debug.Log("게임 종료!");
                isGameOver = true;

                if (DogeOverField != null)
                    DogeOverField.SetActive(true);

                break;
            }
        }
    }
}
