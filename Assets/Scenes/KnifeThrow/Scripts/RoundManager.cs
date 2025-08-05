using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    public int knivesToStick;             // 이번 라운드에 꽂아야 할 칼 개수
    public RotatePattern rotatePattern; // 이번 라운드 회전 패턴
    public List<float> preKnifeAngles;  // 미리 꽂혀있는 칼 개수 (옵션)
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
        //=============라운드 설정=============//

        rounds = new RoundData[]
        {
            new RoundData() { //1라운드 - 기본5
                knivesToStick = 5,
                rotatePattern = RotatePattern.Basic,
                preKnifeAngles= new List<float> { }
            },
            new RoundData() { //2라운드 - 일자5
                knivesToStick = 5,
                rotatePattern = RotatePattern.Fast,
                preKnifeAngles= new List<float> {0f, 180f}
            },
            new RoundData() { //3라운드 - 느린7
                knivesToStick = 7,
                rotatePattern = RotatePattern.Slow,
                preKnifeAngles= new List<float> {90f}
            },
            new RoundData() { //4라운드 - 십자5
                knivesToStick = 7,
                rotatePattern = RotatePattern.Basic,
                preKnifeAngles= new List<float> {0f, 90f, 180f, 270f}
            },
            new RoundData() { //5라운드 - 진동10
                knivesToStick = 10,
                rotatePattern = RotatePattern.Osilate,
                preKnifeAngles= new List<float> { }
            },
            new RoundData() { //6라운드 - 깜빡이7
                knivesToStick = 7,
                rotatePattern = RotatePattern.Blink,
                preKnifeAngles= new List<float> {0f, 120f, 240f}
            },
            new RoundData() { //7라운드 - 빽빽이 진동7
                knivesToStick = 7,
                rotatePattern = RotatePattern.Osilate,
                preKnifeAngles= new List<float> {0f, 45f, 90f, 180f, 225f, 270f}
            },
            new RoundData() { //8라운드 - 크기변하는 5
                knivesToStick = 5,
                rotatePattern = RotatePattern.SizeOsilate,
                preKnifeAngles= new List<float> {0f, 60f, 120f, 180f, 240f, 300f}
            },
            new RoundData() { //9라운드 - 개빠른 10
                knivesToStick = 10,
                rotatePattern = RotatePattern.Superfast,
                preKnifeAngles= new List<float> {}
            },
            new RoundData() { //10라운드 - 공전
                knivesToStick = 10,
                rotatePattern = RotatePattern.Orbit,
                preKnifeAngles= new List<float> {0f, 180f}
            },
        };

        //=============라운드 끝=============//

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

        foreach (Transform child in spinPan.transform) //기존칼 제거
        {
            if (child.CompareTag("KnifeThrow_Knife"))
            {
                Destroy(child.gameObject);
            }
        }

        foreach (float angle in currentRound.preKnifeAngles) //장애물칼 추가
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

    //==잡다한 함수==//

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
        Debug.Log("시간 초과!");
        RoundFail(); // 실패 처리
    }

    public bool CanThrowKnife()
    {
        return knifeUsed < knifeToHit;
    }
}


