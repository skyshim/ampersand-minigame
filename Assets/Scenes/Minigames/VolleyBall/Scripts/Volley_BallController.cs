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
        // �÷��̾ ������ �׻� ���� Ƣ�� �ӵ� ����
        if (collision.gameObject.CompareTag("Volley_Player"))
        {
            Vector2 velocity = rb.velocity;

            // y������ �ʹ� ������ �ּ��� ���� Ƣ���� ����
            if (velocity.y < 7f)
                velocity.y = 7f;

            // �浹 ���⿡ ���� ��¦ �¿� ���� �����ֱ�
            float dirX = collision.relativeVelocity.x > 0 ? 1 : -1;
            velocity.x += dirX * 1.5f;

            rb.velocity = velocity;
        }
        // �ٴ� ���̾ ����� ��
        if (collision.gameObject.CompareTag("Volley_Ground"))
        {
            gameManager.BallHitGround(transform.position.x);
        }
    }
}
