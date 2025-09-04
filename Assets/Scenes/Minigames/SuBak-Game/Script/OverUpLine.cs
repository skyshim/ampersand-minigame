using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverUpLine : MonoBehaviour
{
    private GameResult gr;


    // Start is called before the first frame update
    void Start()
    {
        gr = FindObjectOfType<GameResult>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.position.y < transform.position.y) {
            gr.GameOver();
        }
    }
}
