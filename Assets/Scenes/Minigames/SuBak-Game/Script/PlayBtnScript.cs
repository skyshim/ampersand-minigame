using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBtnScript : MonoBehaviour
{
    private UM um;

    private bool getInput = false;

    void Start() {
        um = FindObjectOfType<UM>();
    }


    // Update is called once per frame
    void Update()
    {
        if (getInput == true && Input.GetMouseButtonDown(0)) {
            um.isPaused = true;
        }
    }

    void TurnOn() {
        gameObject.SetActive(true);
        getInput = true;
    }
}
