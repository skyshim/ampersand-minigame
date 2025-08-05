using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject manager;
    private GameManagerScript managerScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        manager = GameObject.Find("Main Camera&Game Manager");
        managerScript = manager.GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.position.y < -10)
        {
            Destroy(gameObject);
            if (!managerScript.isGameOver) { managerScript.cnt++; }
            Debug.Log(managerScript.cnt);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        managerScript.isGameOver = true;
        Debug.Log("pooped!!!!!!!!!!!!!!!!!!!");
        Destroy(gameObject);
    }
}
