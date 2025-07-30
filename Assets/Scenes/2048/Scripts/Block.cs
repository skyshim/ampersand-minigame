using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private int posCode; // ��� ��ġ �ڵ�
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
                posCode = 0; // �»��
                break;
            case Vector3 pos when pos == new Vector3(1, 0, 0):
                posCode = 1; // ����
                break;
            case Vector3 pos when pos == new Vector3(0, -1, 0):
                posCode = 2; // ���ϴ�
                break;
            case Vector3 pos when pos == new Vector3(1, -1, 0):
                posCode = 3; // ���ϴ�
                break;
            default:
                Debug.LogError("��� ��ġ �ڵ� ����");
                break;
        }
    }
}
