using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverUpLine : MonoBehaviour
{
    private GameResult gr;
    private float entertime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        gr = FindObjectOfType<GameResult>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.position.y < transform.position.y) {
            gr.GameOver();
        }
        entertime = Time.time;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (Time.time - entertime > 0.3f) {
            gr.GameOver();
        }
    }
}
