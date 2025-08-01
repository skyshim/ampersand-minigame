using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2048 : MonoBehaviour
{
    // ��� ������ �ޱ�
    public GameObject block2;
    public GameObject block4;
    public GameObject block8;
    public GameObject block16;
    public GameObject block32;
    public GameObject block64;
    public GameObject block128;
    public GameObject block256;
    public GameObject block512;
    public GameObject block1024;
    public GameObject block2048;


    private Vector3[] posCode = new Vector3[16]; // ��� ��ġ �ڵ� ����Ʈ
    private float[] x = { -1.8f, -0.6f, 0.6f, 1.8f }; // x��ġ�ڵ�
    private float[] y = { 0.3f, -0.9f, -2.1f, -3.3f }; // y��ġ�ڵ�
    List<bool> isBlock = new List<bool>(); // ��� ����Ʈ
    private int[] blockValue = new int[16];

    public bool playing = false;
    // Start is called before the first frame update
    void Start() 
    {
        // ��� ��ġ �ڵ� ����
        int idx = 0;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                posCode[idx] = new Vector3(x[i], y[j], -2f); // ��� ��ġ �ڵ� ����
                idx++;
            }
        }

        // ��� ����Ʈ �ʱ�ȭ
        for (int i = 0; i < 16; i++) {
            isBlock.Add(false); // ����� �ִ��� ������ �ʱ�ȭ
            blockValue[i] = 0; // ��� �� �ʱ�ȭ
        }


    }

    // Update is called once per frame
    void Update() 
    {
        
        if (Input.anyKeyDown && (playing == false)) { // ���� ����   
            GameStart();
        }

        // �Է¹ޱ�
        bool Linput = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow); 
        bool Rinput = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool Uinput = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow); 
        bool Dinput = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);



    }


    // ���� ���� ����
    void GameStart() {
        CreateBlock();
        CreateBlock(); // ��� 2�� ����
        playing = true;

    }


    // ��� ���� �Լ�
    public void CreateBlock() 
    {
        int spawnPosCode; // ��� ���� ��ġ �ڵ�
        // ��� ���� ��ġ Ž��
        int idx = 0;
        while(true) {
            spawnPosCode = Random.Range(0, 16); // 0~15 ������ ���� �ڵ� ����
            if (isBlock[spawnPosCode] == false) { // ����� ���� ��ġ�� ã��
                break; // ��� ���� ��ġ Ž�� ����
            }
            if (idx > 100) { // ��� ���� ��ġ Ž�� ����
                Debug.Log("��� ���� ��ġ Ž�� ����");
                return; // ��� ���� ��ġ Ž�� ���н� �Լ� ����
            }
            idx++;
        }
        Vector3 spawnPos = posCode[spawnPosCode];

        // ��� ����
        int spawnWhat = Random.Range(0, 2);
        if (spawnWhat == 0) { // 2��� ����
            Instantiate(block2, spawnPos, Quaternion.identity);
            blockValue[spawnPosCode] = 2; // ��� �� ����
        } else { // 4��� ����
            Instantiate(block4, spawnPos, Quaternion.identity);
            blockValue[spawnPosCode] = 4; // ��� �� ����
        }
        isBlock[spawnPosCode] = true; // ��� ������ �˸� true
    }
}
