using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContinueBtnScript : MonoBehaviour
{
    public UnityEvent onClick;
    private UM um;

    private bool getInput = false;

    void Start() {
        um = FindObjectOfType<UM>();
    }

    private void OnEnable() {
        getInput = true;
    }

    private void OnDisable() {
        getInput = false;
    }

    private void OnMouseDown() {
        if (getInput) {
            Onclick();
        }
    }

    public void Onclick() {
        onClick.Invoke();
    }
}
