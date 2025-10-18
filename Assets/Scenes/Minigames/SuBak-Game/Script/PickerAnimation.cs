using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerAnimation : MonoBehaviour
{
    private Hand hand;
    [SerializeField] Animator picker;

    void Start()
    {
        hand = FindObjectOfType<Hand>();
    }

    void Update()
    {
        if (hand.dropSig) {
            picker.SetBool("letItGo", true);
        }
        else {
            picker.SetBool("letItGo", false);
        }
    }
}
