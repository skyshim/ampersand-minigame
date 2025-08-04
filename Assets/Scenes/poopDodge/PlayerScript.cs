using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Button LeftButton;
    public Button RightButton;

    public float moveSpeed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        LeftButton.onClick.AddListener(MoveLeft);
        RightButton.onClick.AddListener(MoveRight);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { MoveLeft(); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { MoveRight(); }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        //// 감속 계수
        //float deceleration = 0.9f;

        //// 현재 속도에 감속 적용
        //rb.velocity = new Vector2(rb.velocity.x * deceleration, rb.velocity.y);
        while (rb.velocity.x != 0)
        {
            if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x + Time.deltaTime * moveSpeed, 0);
            }
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x - Time.deltaTime * moveSpeed, 0);
            }
        }
    }

    void MoveLeft()
    {
        Debug.Log("left");
        rb.velocity = new Vector2(-moveSpeed, 0);
    }
    void MoveRight()
    {
        Debug.Log("Right");
        rb.velocity = new Vector2(moveSpeed, 0);
    }
}
