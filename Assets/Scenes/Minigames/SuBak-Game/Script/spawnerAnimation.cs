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

        spawnerAnim.SetInteger("level", level);
        lv = level;
        switch (level) {
            case 1:
                spawnerAnim.Play("lv1");
                break;
            case 2:
                spawnerAnim.Play("lv2");
                break;
            case 3:
                spawnerAnim.Play("lv3");
                break;
            case 4:
                spawnerAnim.Play("lv4");
                break;
            case 5:
                spawnerAnim.Play("lv5");
                break;
        }
        Invoke("Test", 0.01f);
    }

    private void Test() {
        Debug.Log(spawnerAnim.GetInteger("level") + "_" + lv);
        foreach (AnimatorControllerParameter p in spawnerAnim.parameters) {
            Debug.Log($"{p.name} : {p.type}");
        }
    }
}