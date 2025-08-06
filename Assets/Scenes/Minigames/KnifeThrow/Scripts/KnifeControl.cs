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

                roundManager.ThrowKnifeCount(false);
                roundManager.RoundFail();
            }
            // �̹� �����ְų�, ������ Į���� �ε��� ��� ���� ó�� ����
            return; // �� ��� ���� �浹 ó�� �� ��

        }

        //���� �浹
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

        Vector2 direction = transform.up; // Į�� ���ϴ� ����

        // 1. ���� ��ġ ���: ���� �߽ɿ��� �������� ������ �Ÿ���ŭ ������ ����
        Vector2 desiredWorldPos = (Vector2)panTransform.position - direction * circleRadius;

        // 2. �θ� ���� �� ���� ��ġ, ȸ�� ����
        Vector3 oldWorldPos = transform.position;
        Quaternion oldWorldRot = transform.rotation;

        // ��ġ�� �̸� �����
        transform.position = desiredWorldPos;

        // 3. �θ� ����
        transform.SetParent(panTransform);

        // 4. �θ� �������� ���� ��ġ/ȸ�� ��ȭ�� ����
        transform.position = desiredWorldPos;  // �θ� ���� �Ŀ��� ��ġ ����
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction); // transform.up = direction�� ����

        // ���� Į�� ���� �ڽ����� ���� ���� ���� ���� ��ġ�� ��������
    }

}
