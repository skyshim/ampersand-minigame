using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResetBtnScript : MonoBehaviour
{
    public GameObject rankingPanel;
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
            rankingPanel.SetActive(false);
            Onclick();
        }
    }

    public void Onclick() {
        onClick.Invoke();
    }
}
