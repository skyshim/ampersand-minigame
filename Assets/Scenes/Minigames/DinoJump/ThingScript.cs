using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingScript : MonoBehaviour
{
    public GameObject manager;
    public DinoJumpManagerScript managerScript;
    public Rigidbody2D rb;

    public int thingType;
    public float x, y;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Main Camera&DinoJump Manager");
        managerScript = manager.GetComponent<DinoJumpManagerScript>();
        x = Random.Range(1f, 2f);
        y = Random.Range(1.5f, 2f);
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(x, y, 1);
    }


    // Update is called once per frame
    void Update()
    {
        if (!managerScript.isGameovered && managerScript.isGamestarted)
        {
            rb.velocity = new Vector2(managerScript.gameSpeed * -7, rb.velocity.y);
        }
        else { rb.velocity = new Vector2(0, 0); }

        if (rb.position.x < -11)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { managerScript.isGameovered = true; }
    }
}
    