using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_MoleSpawner : MonoBehaviour
{
    public GameObject[] molePrefabs; // 0: 빨강 두더지, 1: 파랑 두더지, 2: 빨강 폭탄, 3: 파랑 폭탄
    public Transform[] spawnPoints;

    private bool[] holeOccupied;
    private Coroutine spawnCoroutine;

    private Queue<MoleHistoryEntry> recentMoleHistory = new Queue<MoleHistoryEntry>();
    private int recentRedCount = 0;
    private int recentBlueCount = 0;
    [SerializeField] private float historyWindow = 5f; // 몇 초간 기록 유지

    // 히스토리 엔트리 구조체
    [System.Serializable]
    public struct MoleHistoryEntry
    {
        public int moleType; // 0=Red, 1=Blue
        public float timestamp;
    }

    void Awake()
    {
        holeOccupied = new bool[spawnPoints.Length];
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    IEnumerator SpawnRoutine()
    {
        float baseInterval = 1.5f;
        float interval = baseInterval;
        float elapsedTime = 0f;

        while (true)
        {
            SpawnMole(elapsedTime);

            // ±20% 랜덤값 구하기
            float randomFactor = Random.Range(0.8f, 1.2f);
            interval = baseInterval * randomFactor;

            yield return new WaitForSeconds(interval);

            elapsedTime += interval;

            // 점점 빠르게 (최소 0.3초까지)
            if (baseInterval > 0.3f)
                baseInterval *= 0.96f;
        }
    }

    void SpawnMole(float elapsedTime)
    {
        // 오래된 히스토리 정리
        CleanupOldHistory(elapsedTime);

        // 빈 구멍 찾기
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < holeOccupied.Length; i++)
        {
            if (!holeOccupied[i])
                availableIndices.Add(i);
        }

        if (availableIndices.Count == 0)
            return;

        int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        Transform spawnPoint = spawnPoints[randomIndex];

        // 폭탄 확률 계산 (시간에 따라 증가, 최대 50%)
        float bombChance = Mathf.Clamp01(elapsedTime / 40f * 0.5f);

        int moleIndex;
        GameObject selectedPrefab;

        if (Random.value < bombChance)
        {
            // 폭탄 생성
            moleIndex = Random.Range(2, 4);
            selectedPrefab = molePrefabs[moleIndex];
        }
        else
        {
            // 일반 두더지 생성 - 밸런싱 적용
            float baseWeight = 1f;
            float redWeight = baseWeight;
            float blueWeight = baseWeight;

            // 최근 생성된 두더지 비율에 따른 가중치 조정
            int diff = recentRedCount - recentBlueCount;

            if (diff > 2) blueWeight += 0.5f;       // 빨강이 너무 많으면 파랑 확률 증가
            else if (diff < -2) redWeight += 0.5f;  // 파랑이 너무 많으면 빨강 확률 증가

            float totalWeight = redWeight + blueWeight;
            float rand = Random.Range(0f, totalWeight);

            if (rand < redWeight)
            {
                moleIndex = 0; // Red Mole
            }
            else
            {
                moleIndex = 1; // Blue Mole
            }

            selectedPrefab = molePrefabs[moleIndex];

            // 히스토리에 추가 (폭탄은 제외)
            AddToHistory(moleIndex, elapsedTime);
        }

        // 두더지 생성
        GameObject mole = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

        // 타입 설정 (prefab에 이미 설정되어 있다면 이 부분 제거 가능)
        Mole_Mole moleScript = mole.GetComponent<Mole_Mole>();
        if (moleScript != null)
        {
            switch (moleIndex)
            {
                case 0: moleScript.moleType = MoleType.RedMole; break;
                case 1: moleScript.moleType = MoleType.BlueMole; break;
                case 2: moleScript.moleType = MoleType.RedBomb; break;
                case 3: moleScript.moleType = MoleType.BlueBomb; break;
            }
        }

        // 구멍 점유 상태 관리
        holeOccupied[randomIndex] = true;
        StartCoroutine(ReleaseHoleAfterDelay(randomIndex, 1.0f));

        // 두더지 자동 제거
        Destroy(mole, 1.0f);
    }

    void AddToHistory(int moleType, float currentTime)
    {
        // 일반 두더지만 히스토리에 추가 (0: Red, 1: Blue)
        if (moleType == 0 || moleType == 1)
        {
            MoleHistoryEntry entry = new MoleHistoryEntry
            {
                moleType = moleType,
                timestamp = currentTime
            };

            recentMoleHistory.Enqueue(entry);

            if (moleType == 0) recentRedCount++;
            else if (moleType == 1) recentBlueCount++;
        }
    }

    void CleanupOldHistory(float currentTime)
    {
        // historyWindow 시간보다 오래된 기록 제거
        while (recentMoleHistory.Count > 0)
        {
            MoleHistoryEntry oldEntry = recentMoleHistory.Peek();
            if (currentTime - oldEntry.timestamp > historyWindow)
            {
                recentMoleHistory.Dequeue();
                if (oldEntry.moleType == 0) recentRedCount--;
                else if (oldEntry.moleType == 1) recentBlueCount--;
            }
            else
            {
                break; // 큐는 시간순으로 정렬되어 있으므로 더 이상 확인할 필요 없음
            }
        }
    }

    IEnumerator ReleaseHoleAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        holeOccupied[index] = false;
    }
}