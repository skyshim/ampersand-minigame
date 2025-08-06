using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogePlayer : MonoBehaviour
{
    private float doge_horizontal;
    private float doge_vertical;
    private float doge_speed = 50f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        doge_horizontal = Input.GetAxis("Horizontal");
        doge_vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(doge_horizontal * doge_speed, doge_vertical * doge_speed);
    }
}
