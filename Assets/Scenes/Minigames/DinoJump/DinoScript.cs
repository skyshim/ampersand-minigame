using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class DinoScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject manager;

    public float force = 10f;
    public float normalG = 3f;
    public float reducedG = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DinoJumpManagerScript managerScript = manager.GetComponent<DinoJumpManagerScript>();

        if (!managerScript.isGameovered && managerScript.isGamestarted) 
        {
            if (Input.touchCount > 0)
            {
                jump();
                Touch touch = Input.GetTouch(0);

                rb.gravityScale = touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary ? reducedG : normalG;
            }
            if (Input.GetMouseButton(0))
            {
                Debug.Log("마우스클릭함");
                jump();

                rb.gravityScale = Input.GetMouseButton(0) ? reducedG : normalG;
            }
            else
                rb.gravityScale = normalG;
        }
        else { rb.velocity = new Vector2(0, 0); }
    }

    public void jump()
    {
        if (rb.position.y <= -3) { rb.velocity = new Vector2(rb.velocity.x, force); }
    }
}
