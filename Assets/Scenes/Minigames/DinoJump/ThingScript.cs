using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingScript : MonoBehaviour
{
    public GameObject manager;
    public DinoJumpManagerScript managerScript;
    public Rigidbody2D rb;
    public Sprite[] obstacleSprites;
    private SpriteRenderer sr;


    public int thingType;
    public float x, y;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Main Camera&DinoJump Manager");
        managerScript = manager.GetComponent<DinoJumpManagerScript>();
        x = Random.Range(1f, 1.8f);
        y = Random.Range(0.5f, 1.8f);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        transform.localScale = new Vector3(x, y, 1);
        transform.rotation = Quaternion.identity;
        sr.color = Color.white;

        if (x <= 1.3f)
        {
            if (y <= 1f) sr.sprite = obstacleSprites[3];
            else if (y <= 1.5) sr.sprite = obstacleSprites[0];
            else sr.sprite = obstacleSprites[4];
        }
        else if (x <= 1.6f)
        {
            if (y <= 1.6f) sr.sprite = obstacleSprites[0];
            else sr.sprite = obstacleSprites[1];
        }
        else
        {
            if (y <= 1.5f) sr.sprite = obstacleSprites[2];
            else sr.sprite = obstacleSprites[1];
        }
        PolygonCollider2D pc = GetComponent<PolygonCollider2D>();
        Sprite currentSprite = sr.sprite;

        pc.pathCount = currentSprite.GetPhysicsShapeCount();
        for (int i = 0; i < pc.pathCount; i++)
        {
            var path = new List<Vector2>();
            currentSprite.GetPhysicsShape(i, path);
            pc.SetPath(i, path.ToArray());
        }
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
    