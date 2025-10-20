using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerAnimation : MonoBehaviour
{
    [SerializeField] Animator spawnerAnim;
    [SerializeField] Hand hand;

    public void ChangeShape(int level) {
        if (spawnerAnim == null) {
            Debug.LogError("spawnerAnim이 연결되어 있지 않습니다!");
            return;
        }

        spawnerAnim.SetInteger("i", level);
        Debug.Log(spawnerAnim.GetInteger("i") + "_" + level);   
    }
}