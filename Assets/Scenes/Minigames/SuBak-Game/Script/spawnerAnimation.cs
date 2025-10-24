using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class spawnerAnimation : MonoBehaviour
{
    [SerializeField] Animator spawnerAnim;
    [SerializeField] Hand hand;
    private SpriteRenderer sr;

    int lv;
    
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }


    public void ChangeShape(int level) {
        spawnerAnim.SetInteger("level", level);
    }

    public void TurnON() {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f); // TurnON
    }

    public void TurnOFF() {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f); // TurnOFF
    }
}