using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogeBlock1 : MonoBehaviour
{
    private Vector2 direction;
    private float doge_block_speed = 70f;
    private Collider2D myCollider;

    private static bool allStopped = false;

    public static bool AllStopped
    {
        get { return allStopped; }
    }

    void Start()
    {
        allStopped = false; 
        myCollider = GetComponent<Collider2D>();

        direction = (Vector2.left + Vector2.up).normalized;

        GameObject[] excludedWalls = {
            GameObject.Find("DogeWall(North)"),
            GameObject.Find("DogeWall(South)"),
            GameObject.Find("DogeWall(East)"),
            GameObject.Find("DogeWall(West)"),
            GameObject.Find("DogeBlock1"),
            GameObject.Find("DogeBlock1 (1)"),
            GameObject.Find("DogeBlock1 (2)"),
            GameObject.Find("DogeBlock1 (3)"),
        };

        foreach (GameObject wall in excludedWalls)
        {
            if (wall == null) continue;
            Collider2D col = wall.GetComponent<Collider2D>();
            if (col != null)
            {
                Physics2D.IgnoreCollision(myCollider, col, true);
            }
        }
    }

    void Update()
    {
        if (allStopped)
            return;

        transform.Translate(direction * doge_block_speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        string name = collision.gameObject.name;

        if (name == "DogeWall(North) (1)")
        {
            direction = Vector2.Reflect(direction, Vector2.down);
            Debug.Log("North wall hit → reflected down");
        }
        else if (name == "DogeWall(South) (1)")
        {
            direction = Vector2.Reflect(direction, Vector2.up);
            Debug.Log("South wall hit → reflected up");
        }
        else if (name == "DogeWall(East) (1)")
        {
            direction = Vector2.Reflect(direction, Vector2.left);
            Debug.Log("East wall hit → reflected left");
        }
        else if (name == "DogeWall(West) (1)")
        {
            direction = Vector2.Reflect(direction, Vector2.right);
            Debug.Log("West wall hit → reflected right");
        }
        else if (name == "DogePlayer")
        {
            allStopped = true;
            direction = Vector2.zero;
            Debug.Log("Player hit → ALL blocks stopped");
        }
    }
}
