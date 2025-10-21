using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class spawnerAnimation : MonoBehaviour
{
    [SerializeField] Animator spawnerAnim;
    [SerializeField] Hand hand;

    int lv;

    public void ChangeShape(int level) {
        if (spawnerAnim == null) {
            Debug.LogError("spawnerAnim이 연결되어 있지 않습니다!");
            return;
        }

        spawnerAnim.SetInteger("i", level);
        lv = level;
        Invoke("Test", 0.01f);
    }

    private void Test() {
        Debug.Log(spawnerAnim.GetInteger("i") + "_" + lv);
    }
}