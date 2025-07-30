using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RoundData
{
    public int knivesToStick;             // �̹� ���忡 �Ⱦƾ� �� Į ����
    public int rotatePattern; // �̹� ���� ȸ�� ����
    public List<float> preKnifeAngles;           // �̸� �����ִ� Į ���� (�ɼ�)
}

public class RoundManager : MonoBehaviour
{
    public PanRotate spinPan;
    public KnifeUI knifeUI;
    [SerializeField] private GameObject Knife;

    public RoundData[] rounds;
    public int knifeUsed = 0;
    public int knifeToHit = 0;

    private int index = 0;
    

    void Start()
    {
        //=============���� ����=============//

        rounds = new RoundData[]
        {
            new RoundData() { //1����
                knivesToStick = 5,
                rotatePattern = 1,
                preKnifeAngles= new List<float> { }
            },
            new RoundData() { //2����
                knivesToStick = 5,
                rotatePattern = 1,
                preKnifeAngles= new List<float> {0f, 180f}
            },
            new RoundData() { //3����
                knivesToStick = 7,
                rotatePattern = 2,
                preKnifeAngles= new List<float> { }
            },
            new RoundData() { //4����
                knivesToStick = 7,
                rotatePattern = 3,
                preKnifeAngles= new List<float> {0f, 90f, 180f, 270f}
            },

        };

        //=============���� ��=============//

        StartRound();
    }

    public void StartRound()
    {
        RoundData currentRound = rounds[index];
        knifeToHit = currentRound.knivesToStick;

        knifeUI.SetKnives(currentRound.knivesToStick);
        spinPan.SetPattern(currentRound.rotatePattern);

        foreach (Transform child in spinPan.transform) //����Į ����
        {
            if (child.CompareTag("KnifeThrow_Knife"))
            {
                Destroy(child.gameObject);
            }
        }

        foreach (float angle in currentRound.preKnifeAngles)
        {
            GameObject knife = Instantiate(Knife, spinPan.transform);

            float radius = 1f; // Į�� �󸶳� Ƣ�����
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
            Vector2 position = (Vector2)spinPan.transform.position + direction * radius;

            knife.transform.position = position;
            knife.transform.up = direction;
        }
    }

    public void ThrowKnifeCount()
    {
        knifeUsed++;
        if (knifeUsed == knifeToHit)
        {
            RoundSuccess();
        }
    }

    public void RoundSuccess()
    {
        Debug.Log("clear");
        index++;
        Invoke("StartRound", 3f);
    }

    public void RoundFail()
    {
        Debug.Log("End");
    }
}


