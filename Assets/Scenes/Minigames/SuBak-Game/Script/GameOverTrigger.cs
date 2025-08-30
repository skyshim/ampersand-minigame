//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class GameOverTrigger : MonoBehaviour {
//    private GameManager gameManager;
//    private Transform ts;
//    float posY;

//    void Start() {
//        posY = transform.position.y;
//        gameManager = FindObjectOfType<GameManager>();
//    }

//    void OnTriggerEnter2D(Collider2D other) {
//        if (other.GetComponent<FruitController>() != null) {
//            if (other.transform.position.y < posY) {
//                if (gameManager != null) {
//                    gameManager.GameOver();
//                }
//            }
//        }
//    }
//}