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
        Debug.Log("�浹 ����: " + collision.gameObject.name);

        //Į���� �浹
        if (collision.gameObject.CompareTag("KnifeThrow_Knife"))
        {
            if (!isCollide && collision.gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic)
            {
                isCollide = true;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;  // ���� �ӵ� �ʱ�ȭ

                Vector2 awayFromStuckKnife = (transform.position - collision.transform.position).normalized;

                rb.AddForce(awayFromStuckKnife * bounceForce, ForceMode2D.Impulse);
                Destroy(gameObject, 1f);

                roundManager.RoundFail();
            }
            // �̹� �����ְų�, ������ Į���� �ε��� ��� ���� ó�� ����
            return; // �� ��� ���� �浹 ó�� �� ��
        }

        //���� �浹
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
