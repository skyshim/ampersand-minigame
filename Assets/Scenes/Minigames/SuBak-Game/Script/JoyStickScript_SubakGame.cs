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


    private void OnMouseDown() {
        Color c = ren.color;
        c.a = 1f;
        ren.color = c;

        switch (gameObject.name) {
            case "left": hand.isleft = true; ren.color = c; break;
            case "right": hand.isright = true; ren.color = c; break;
            case "down":
                hand.isdown = true;
                Invoke("DownBtnOn", hand.clickDelay - 0.01f);
                break;
        }
    }

    private void OnMouseUp() {
        Color c = ren.color;
        c.a = 0f;
        ren.color = c;
        switch (gameObject.name) {
            case "left": hand.isleft = false; ren.color = c; break;
            case "right": hand.isright = false; ren.color = c; break;
            case "down": hand.isdown = false; ren.color = c; break;
        }
    }

    private void DownBtnOn() {
        Color c = ren.color;
        c.a = 0f;
        ren.color = c;
    }
}
