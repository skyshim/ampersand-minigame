using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JoyStickScript_SubakGame : MonoBehaviour
{
    [SerializeField] private Hand hand;
    private SpriteRenderer ren;

    private void Start() {
        ren = GetComponent<SpriteRenderer>();
        
    }

    private void OnMouseOver() {
        Color c = ren.color;
        c.a = 1f;

        switch (gameObject.name) {
            case "left": hand.isleft = true; ren.color = c; break;
            case "right": hand.isright = true; ren.color = c; break;
        }
    }
    private void OnMouseExit() {
        Color c = ren.color;
        c.a = 0f;

        switch (gameObject.name) {
            case "left": hand.isleft = false; ren.color = c; break;
            case "right": hand.isright = false; ren.color = c; break;
        }
    }

    private void OnMouseDown() {
        if (gameObject.name == "down") {
            hand.isdown = true;
            Color c = ren.color;
            c.a = 1f;
            ren.color = c;
            Invoke("DownBtnOn", hand.clickDelay-0.01f);
        }
    }

    private void DownBtnOn() {
        Color c = ren.color;
        c.a = 0f;
        ren.color = c;
    }
}
