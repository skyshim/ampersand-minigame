using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private int posCode; // 블록 위치 코드
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BlockPosition() 
    {
        Vector3 curpos = transform.position;
        switch (curpos) {
            case Vector3 pos when pos == new Vector3(0, 0, 0):
                posCode = 0; // 좌상단
                break;
            case Vector3 pos when pos == new Vector3(1, 0, 0):
                posCode = 1; // 우상단
                break;
            case Vector3 pos when pos == new Vector3(0, -1, 0):
                posCode = 2; // 좌하단
                break;
            case Vector3 pos when pos == new Vector3(1, -1, 0):
                posCode = 3; // 우하단
                break;
            default:
                Debug.LogError("블록 위치 코드 오류");
                break;
        }
    }
}
