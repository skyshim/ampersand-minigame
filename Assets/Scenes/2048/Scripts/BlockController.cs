using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Xinput = Input.GetAxisRaw("Horizontal");
        float Yinput = Input.GetAxisRaw("Vertical");

        if (Xinput == -1) {
            MoveA();
        }
        //if (Xinput == 1) {
        //    MoveD();
        //}
        //if (Yinput == -1) {
        //    MoveS();
        //}
        //if (Yinput == 1) {
        //    MoveW();
        //}
    }

    
    void MoveA() // �·� �̵�
    {
        // ��� ã��
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Blocks_2048");
        int blockCount = blocks.Length;

        // ��� �̵�


    }

}
