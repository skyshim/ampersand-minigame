using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    [HideInInspector] public float moveDirection = 0f;
    private Rigidbody2D rb;
    public GameObject manager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GameManagerScript managerScript = manager.GetComponent<GameManagerScript>();

        if (!managerScript.isGameOver) { rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y); }
    }
}
