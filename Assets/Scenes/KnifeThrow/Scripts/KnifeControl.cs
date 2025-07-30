using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeControl : MonoBehaviour
{
    public RoundManager roundManager;

    public float speed = 15f;
    public bool isCollide = false;
    public float bounceForce = 50f;

    private float circleRadius;
                    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCollide)
        {
            transform.Translate(Vector2.up * speed *  Time.deltaTime);
        }
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

                roundManager.RoundFail();
            }
            // 이미 꽂혀있거나, 던지는 칼끼리 부딪힌 경우 별도 처리 가능
            return; // 이 경우 원판 충돌 처리 안 함
        }

        //원판 충돌
        if (!isCollide && collision.gameObject.CompareTag("KnifeThrow_SpinPan"))
        {
            isCollide = true;
            
            Transform circleTransform = collision.transform;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;


            CircleCollider2D circleCollider = circleTransform.GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                circleRadius = circleCollider.radius * circleTransform.localScale.x;
            }
            else
            {
                circleRadius = 1f;
            }
            
            Vector2 attachPos = (Vector2)circleTransform.position + Vector2.up * circleRadius;
            transform.SetParent(circleTransform);
        }


    }
}
