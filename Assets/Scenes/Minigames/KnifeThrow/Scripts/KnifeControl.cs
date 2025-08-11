using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeControl : MonoBehaviour
{
    public RoundManager roundManager;

    public float speed = 15f;
    public bool isCollide = false;
    public bool isLaunched = false;
    public float bounceForce = 50f;

    private float circleRadius;
                    
    // Start is called before the first frame update
    void Start()
    {
        roundManager = GameObject.Find("GameController").GetComponent<RoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCollide && isLaunched)
        {
            transform.Translate(Vector2.up * speed *  Time.deltaTime);
        }
    }

    public void Launch()
    {
        isLaunched = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌 감지: " + collision.gameObject.name);

        //칼끼리 충돌
        if (collision.gameObject.CompareTag("KnifeThrow_Knife"))
        {
            if (!isCollide && collision.gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic)
            {
                isCollide = true;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;  // 기존 속도 초기화

                Vector2 awayFromStuckKnife = (transform.position - collision.transform.position).normalized;

                rb.AddForce(awayFromStuckKnife * bounceForce, ForceMode2D.Impulse);
                Destroy(gameObject, 1f);

                roundManager.ThrowKnifeCount(false);
                roundManager.RoundFail();
            }
            // 이미 꽂혀있거나, 던지는 칼끼리 부딪힌 경우 별도 처리 가능
            return; // 이 경우 원판 충돌 처리 안 함

        }

        //원판 충돌
        if (!isCollide && collision.gameObject.CompareTag("KnifeThrow_SpinPan"))
        {
            isCollide = true;
            AttachToPan(collision.transform);
            roundManager.ThrowKnifeCount(true);
            FindObjectOfType<CreateKnife>().SpawnKnife();
        }
    }
    public void AttachToPan(Transform panTransform)
    {
        isCollide = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;

        CircleCollider2D circleCollider = panTransform.GetComponent<CircleCollider2D>();
        if (circleCollider != null)
            circleRadius = circleCollider.radius * panTransform.localScale.x;
        else
            circleRadius = 1f;

        Vector2 direction = transform.up; // 칼이 향하는 방향

        // 1. 월드 위치 계산: 원판 중심에서 방향으로 반지름 거리만큼 떨어진 지점
        Vector2 desiredWorldPos = (Vector2)panTransform.position - direction * circleRadius;

        // 2. 부모 변경 전 월드 위치, 회전 저장
        Vector3 oldWorldPos = transform.position;
        Quaternion oldWorldRot = transform.rotation;

        // 위치를 미리 맞춘다
        transform.position = desiredWorldPos;

        // 3. 부모 변경
        transform.SetParent(panTransform);

        // 4. 부모 변경으로 인한 위치/회전 변화를 보정
        transform.position = desiredWorldPos;  // 부모 변경 후에도 위치 유지
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction); // transform.up = direction과 동일

        // 이제 칼은 원판 자식으로 공전 자전 중일 때도 위치가 안정적임
    }

}
