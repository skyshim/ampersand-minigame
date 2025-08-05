using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    public int knivesToStick;             // �̹� ���忡 �Ⱦƾ� �� Į ����
    public RotatePattern rotatePattern; // �̹� ���� ȸ�� ����
    public List<float> preKnifeAngles;  // �̸� �����ִ� Į ���� (�ɼ�)
}

public class RoundManager : MonoBehaviour
{
    public PanRotate spinPan;
    public KnifeUI knifeUI;
    public RoundUI roundUI;
    public TimeControl timer;
    [SerializeField] private GameObject Knife;

    public RoundData[] rounds;
    public int knifeHit = 0;
    public int knifeUsed = 0;
    public int knifeToHit = 0;

    private int index = 8;
    

    void Start()
    {
        //=============���� ����=============//

        rounds = new RoundData[]
        {
            new RoundData() { //1���� - �⺻5
                knivesToStick = 5,
                rotatePattern = RotatePattern.Basic,
                preKnifeAngles= new List<float> { }
            },
            new RoundData() { //2���� - ����5
                knivesToStick = 5,
                rotatePattern = RotatePattern.Fast,
                preKnifeAngles= new List<float> {0f, 180f}
            },
            new RoundData() { //3���� - ����7
                knivesToStick = 7,
                rotatePattern = RotatePattern.Slow,
                preKnifeAngles= new List<float> {90f}
            },
            new RoundData() { //4���� - ����5
                knivesToStick = 7,
                rotatePattern = RotatePattern.Basic,
                preKnifeAngles= new List<float> {0f, 90f, 180f, 270f}
            },
            new RoundData() { //5���� - ����10
                knivesToStick = 10,
                rotatePattern = RotatePattern.Osilate,
                preKnifeAngles= new List<float> { }
            },
            new RoundData() { //6���� - ������7
                knivesToStick = 7,
                rotatePattern = RotatePattern.Blink,
                preKnifeAngles= new List<float> {0f, 120f, 240f}
            },
            new RoundData() { //7���� - ������ ����7
                knivesToStick = 7,
                rotatePattern = RotatePattern.Osilate,
                preKnifeAngles= new List<float> {0f, 45f, 90f, 180f, 225f, 270f}
            },
            new RoundData() { //8���� - ũ�⺯�ϴ� 5
                knivesToStick = 5,
                rotatePattern = RotatePattern.SizeOsilate,
                preKnifeAngles= new List<float> {0f, 60f, 120f, 180f, 240f, 300f}
            },
            new RoundData() { //9���� - ������ 10
                knivesToStick = 10,
                rotatePattern = RotatePattern.Superfast,
                preKnifeAngles= new List<float> {}
            },
            new RoundData() { //10���� - ����
                knivesToStick = 10,
                rotatePattern = RotatePattern.Orbit,
                preKnifeAngles= new List<float> {0f, 180f}
            },
        };

        //=============���� ��=============//

        StartRound();
    }

    public void StartRound()
    {
        knifeHit = 0;
        knifeUsed = 0;
        RoundData currentRound;
        if (index < rounds.Length) {
            currentRound = rounds[index];
        }
        else
        {
            spinPan.baseSpeed += 0.5f;
            currentRound = rounds[Random.Range(0, rounds.Length)];
        }


        knifeToHit = currentRound.knivesToStick;

        knifeUI.SetKnives(currentRound.knivesToStick);
        spinPan.SetPattern(currentRound.rotatePattern);
        roundUI.SetRound(index + 1);

        foreach (Transform child in spinPan.transform) //����Į ����
        {
            if (child.CompareTag("KnifeThrow_Knife"))
            {
                Destroy(child.gameObject);
            }
        }

        foreach (float angle in currentRound.preKnifeAngles) //��ֹ�Į �߰�
        {
            GameObject knife = Instantiate(Knife);
            KnifeControl kc = knife.GetComponent<KnifeControl>();

            float radius = 1f;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
            Vector2 position = (Vector2)spinPan.transform.position + direction * radius;

            knife.transform.position = position;
            knife.transform.up = direction;

            kc.AttachToPan(spinPan.transform);
        }

        timer.StartTimer();
    }

    //==����� �Լ�==//

    public void ThrowKnifeCount(bool isHit)
    {
        if (isHit)
        {
            knifeHit++;
            roundUI.AddScore(Mathf.RoundToInt(700f / knifeToHit));
        }
        knifeUsed++;
        if (knifeUsed == knifeToHit)
        {
            if (knifeHit == knifeToHit)
            {
                RoundSuccess();
            }
            else
            {
                RoundFail();
            }
        }
    }

    public void RoundSuccess()
    {
        Debug.Log("clear");
        index++;

        timer.StopTimer();
        if (timer.currentTime >= 5f)
        {
            roundUI.AddScore(300);
        }
        else
        {
            roundUI.AddScore(Mathf.RoundToInt(timer.currentTime * 60f));
        }
        Invoke("StartRound", 3f);
    }

    public void RoundFail()
    {
        Debug.Log("End");
        timer.StopTimer();
    }

    public void OnTimeOver()
    {
        Debug.Log("�ð� �ʰ�!");
        RoundFail(); // ���� ó��
    }

    public bool CanThrowKnife()
    {
        return knifeUsed < knifeToHit;
    }
}


