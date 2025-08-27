using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject manager;
    private GameManagerScript managerScript;
    public Sprite floorpoop;
    private SpriteRenderer spriteRenderer;

    public bool isOnFloor = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        manager = GameObject.Find("Main Camera&Game Manager");
        managerScript = manager.GetComponent<GameManagerScript>();
        rb.velocity = new Vector2(0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.position.y <= -7.5f && !isOnFloor)
        {
            isOnFloor = true;
            spriteRenderer.sprite = floorpoop;
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0f;
            Destroy(gameObject, 1f);
            if (!managerScript.isGameOver && managerScript.isGameStart) { managerScript.cnt++; }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOnFloor && collision.CompareTag("Player")) {
            managerScript.isGameOver = true;
            Destroy(gameObject);
        }
    }
}
