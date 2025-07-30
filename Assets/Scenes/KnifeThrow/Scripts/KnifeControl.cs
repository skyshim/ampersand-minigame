using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeControl : MonoBehaviour
{
    public float speed = 15f;
    public bool isCollide = false;

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
        if (!isCollide && collision.gameObject.CompareTag("SpinPan"))
        {
            isCollide = true;

            Transform circleTransform = collision.transform;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

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

            // 부모를 원형 오브젝트로 변경
            transform.SetParent(circleTransform);
        }
        else if (isCollide && collision.gameObject.CompareTag("Knife"))
        {
            isCollide = true;
            Destroy(gameObject, 1f);
        }
    }
}
