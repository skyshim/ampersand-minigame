using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_1945 : MonoBehaviour {
    private bool gogo = false;
    private float speed;
    public string whoSpawn; 

    // Start is called before the first frame update
    private void OnEnable() {
        gogo = true;
        switch (whoSpawn) {
            case "player": speed = 10f; break;
            case "enemy_Gun": speed = 6f; break;
        }
    }

    private void Update() {
        if (gogo == true) {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "enemy_1945" || collision.gameObject.tag == "bullet-e_1945" || collision.gameObject.tag == "boundary_1945") {
            gogo = false;
            Destroy(gameObject);
        }
    }
}
