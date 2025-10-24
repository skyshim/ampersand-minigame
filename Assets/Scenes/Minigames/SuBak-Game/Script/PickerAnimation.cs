using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerAnimation : MonoBehaviour
{
    [SerializeField] private Hand hand;
    [SerializeField] Animator picker;

    void Start()
    {
        hand = FindObjectOfType<Hand>();

        picker.SetBool("letItGo", false);
    }

    

    void Update()
    {
        if (hand.dropSig) {
            picker.SetBool("letItGo", true);
            foreach (AnimatorControllerParameter p in picker.parameters) {
                Debug.Log($"{p.name} : {p.type}");
            }
        }
        else {
            picker.SetBool("letItGo", false);
        }
    }
}
