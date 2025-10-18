using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class DinoScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject manager;
    public DinoJumpManagerScript managerScript;
    private SpriteRenderer sr;
    private PolygonCollider2D pc;

    public float force = 10f;
    public float normalG = 3f;
    public float reducedG = 2f;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = GetComponent<DinoJumpManagerScript>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pc = GetComponent<PolygonCollider2D>();
        Sprite sprite = sr.sprite;
        pc.pathCount = sprite.GetPhysicsShapeCount();

        for (int i = 0; i < pc.pathCount; i++)
        {
            var path = new List<Vector2>();
            sprite.GetPhysicsShape(i, path);
            pc.SetPath(i, path.ToArray());
        }

    }

    // Update is called once per frame
    void Update()
    {
        DinoJumpManagerScript managerScript = manager.GetComponent<DinoJumpManagerScript>();

        if (!managerScript.isGameovered && managerScript.isGamestarted && rb.position.y > -5)
        {
            if (Input.touchCount > 0)
            {
                jump();
                Touch touch = Input.GetTouch(0);

                rb.gravityScale = touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary ? reducedG : normalG;
            }
            if (Input.GetMouseButton(0))
            {
                jump();

                rb.gravityScale = Input.GetMouseButton(0) ? reducedG : normalG;
            }
            else
                rb.gravityScale = normalG;
        }
        else if (rb.position.y < -5) managerScript.isGameovered = true;
        else { rb.velocity = new Vector2(0, 0); }
    }

    public void jump()
    {
        if (rb.position.y <= -3.2f) { rb.velocity = new Vector2(rb.velocity.x, force); }
    }
}
