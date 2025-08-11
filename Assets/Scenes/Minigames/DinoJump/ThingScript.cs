using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingScript : MonoBehaviour
{
    public GameObject manager;
    public Rigidbody2D rb;

    public int thingType;
    public float x, y;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Main Camera&DinoJump Manager");
        x = Random.Range(0.5f, 2.5f);
        y = Random.Range(1f, 1.5f);
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(x, y, 1);
    }


    // Update is called once per frame
    void Update()
    {
        DinoJumpManagerScript managerScript = manager.GetComponent<DinoJumpManagerScript>();

        if (!managerScript.isGameovered && managerScript.isGamestarted)
        {
            rb.velocity = new Vector2(managerScript.gameSpeed * -7, -5);
        }
        else { rb.velocity = new Vector2(0, 0); }

        if (rb.position.x < -11)
        {
            Destroy(gameObject);
        }
    }
}
