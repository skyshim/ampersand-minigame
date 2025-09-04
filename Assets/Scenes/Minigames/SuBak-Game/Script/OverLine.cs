using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverLine : MonoBehaviour
{
    private GameResult gr;


    // Start is called before the first frame update
    void Start() {
        gr = FindObjectOfType<GameResult>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Fruit")) {
            gr.GameOver();
        }
    }
}
