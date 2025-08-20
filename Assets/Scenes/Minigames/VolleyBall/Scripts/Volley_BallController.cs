using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volley_BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Volley_GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어에 맞으면 항상 위로 튀게 속도 보정
        if (collision.gameObject.CompareTag("Volley_Player"))
        {
            Vector2 velocity = rb.velocity;

            // y성분이 너무 낮으면 최소한 위로 튀도록 보정
            if (velocity.y < 7f)
                velocity.y = 7f;

            // 충돌 방향에 따라 살짝 좌우 힘도 더해주기
            float dirX = collision.relativeVelocity.x > 0 ? 1 : -1;
            velocity.x += dirX * 1.5f;

            rb.velocity = velocity;
        }
        // 바닥 레이어에 닿았을 때
        if (collision.gameObject.CompareTag("Volley_Ground"))
        {
            gameManager.BallHitGround(transform.position.x);
        }
    }
}
