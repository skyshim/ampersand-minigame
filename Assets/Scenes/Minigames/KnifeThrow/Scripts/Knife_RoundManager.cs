using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[System.Serializable]
public class RoundData
{
    public int knivesToStick;             // �̹� ���忡 �Ⱦƾ� �� Į ����
    public RotatePattern rotatePattern; // �̹� ���� ȸ�� ����
    public List<float> preKnifeAngles;  // �̸� �����ִ� Į ���� (�ɼ�)
}

public class Knife_RoundManager : MonoBehaviour
{
    public Knife_PanRotate spinPan;
    public Knife_CreateKnife createKnife;
    public Knife_KnifeUI knifeUI;
    public Knife_RoundUI roundUI;
    public Knife_TimeControl timer;
    public Knife_RoundResult roundResult;
    [SerializeField] private GameObject Knife;

    public RoundData[] rounds;
    public int knifeHit = 0;
    public int knifeUsed = 0;
    public int knifeToHit = 0;

    public int index = 0;
    private List<float> randomKnifes = new List<float> { 0f, 30f, 45f, 60f, 90f, 120f, 135f, 150f, 180f, 210f, 225f, 240f, 270f, 300f, 315f, 330f };

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
            new RoundData() { //7���� - ������ ����5
                knivesToStick = 5,
                rotatePattern = RotatePattern.Osilate,
                preKnifeAngles= new List<float> {0f, 30f, 60f, 180f, 210f, 240f}
            },
            new RoundData() { //8���� - ũ�⺯�ϴ� 5
                knivesToStick = 5,
                rotatePattern = RotatePattern.SizeOsilate,
                preKnifeAngles= new List<float> {0f, 60f, 120f, 180f, 240f, 300f}
            },
            new RoundData() { //9���� - ������ 10
                knivesToStick = 10,
                rotatePattern = RotatePattern.Superfast,
                preKnifeAngles= new List<float> {0f, 180f}
            },
            new RoundData() { //10���� - ����
                knivesToStick = 7,
                rotatePattern = RotatePattern.Orbit,
                preKnifeAngles= new List<float> {0f}
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
        if (index < 10) {
            currentRound = rounds[index];
        }
        else
        {
            spinPan.baseSpeed += 0.5f;
            int knifeCount = Random.Range(0, 5);
            List<float> preKnife = new List<float>();

            for (int i=0; i < knifeCount; i++)
            {
                preKnife.Add(randomKnifes[Random.Range(0, randomKnifes.Count)]);
            }
            currentRound = new RoundData()
            {
                knivesToStick = (new List<int> { 5, 7, 10 })[Random.Range(0, 3)],
                rotatePattern = (RotatePattern)Random.Range(1, 9),
                preKnifeAngles = preKnife
            };
        }

        roundResult.HideAll();
        spinPan.gameObject.SetActive(true);

        knifeToHit = currentRound.knivesToStick;

        knifeUI.SetKnives(currentRound.knivesToStick);
        spinPan.SetPattern(currentRound.rotatePattern);
        roundUI.SetRound(index + 1);

        createKnife.SpawnKnife();

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
            Knife_KnifeControl kc = knife.GetComponent<Knife_KnifeControl>();

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
        if (knifeHit == knifeToHit)
        {
            RoundSuccess();
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
        roundResult.ShowClear();
        Invoke("StartRound", 3f);
    }

    public void RoundFail()
    {
        StartCoroutine(RoundFailDelay());
    }

    IEnumerator RoundFailDelay()
    {
        Debug.Log("End");
        timer.StopTimer();
        roundResult.ShowFail();

        yield return new WaitForSeconds(1f);
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


