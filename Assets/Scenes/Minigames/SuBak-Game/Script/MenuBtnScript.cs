using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuBtnScript : MonoBehaviour
{
    public UnityEvent onClick;
    private UM um;
    private GameResult gr;

    private bool getInput = false;

    void Start() {
        um = FindObjectOfType<UM>();
        gr = FindObjectOfType<GameResult>();
    }

    void Update() {
        if (gr.isGameOver) {
            getInput = false;
        } else {
            getInput = true;
        }
    }


    private void OnMouseDown() {
        if (getInput) {
            Onclick();
        }
    }

    public void Onclick() {
        getInput = false;
        onClick.Invoke();
    }
}
