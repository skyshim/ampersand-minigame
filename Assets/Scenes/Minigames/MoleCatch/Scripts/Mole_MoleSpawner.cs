using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_MoleSpawner : MonoBehaviour
{
    public GameObject[] molePrefabs; // 0: ���� �δ���, 1: �Ķ� �δ���, 2: ���� ��ź, 3: �Ķ� ��ź
    public Transform[] spawnPoints;

    private bool[] holeOccupied;
    private Coroutine spawnCoroutine;

    private Queue<MoleHistoryEntry> recentMoleHistory = new Queue<MoleHistoryEntry>();
    private int recentRedCount = 0;
    private int recentBlueCount = 0;
    [SerializeField] private float historyWindow = 5f; // �� �ʰ� ��� ����

    // �����丮 ��Ʈ�� ����ü
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

            // ��20% ������ ���ϱ�
            float randomFactor = Random.Range(0.8f, 1.2f);
            interval = baseInterval * randomFactor;

            yield return new WaitForSeconds(interval);

            elapsedTime += interval;

            // ���� ������ (�ּ� 0.3�ʱ���)
            if (baseInterval > 0.3f)
                baseInterval *= 0.96f;
        }
    }

    void SpawnMole(float elapsedTime)
    {
        // ������ �����丮 ����
        CleanupOldHistory(elapsedTime);

        // �� ���� ã��
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

        // ��ź Ȯ�� ��� (�ð��� ���� ����, �ִ� 50%)
        float bombChance = Mathf.Clamp01(elapsedTime / 40f * 0.5f);

        int moleIndex;
        GameObject selectedPrefab;

        if (Random.value < bombChance)
        {
            // ��ź ����
            moleIndex = Random.Range(2, 4);
            selectedPrefab = molePrefabs[moleIndex];
        }
        else
        {
            // �Ϲ� �δ��� ���� - �뷱�� ����
            float baseWeight = 1f;
            float redWeight = baseWeight;
            float blueWeight = baseWeight;

            // �ֱ� ������ �δ��� ������ ���� ����ġ ����
            int diff = recentRedCount - recentBlueCount;

            if (diff > 2) blueWeight += 0.5f;       // ������ �ʹ� ������ �Ķ� Ȯ�� ����
            else if (diff < -2) redWeight += 0.5f;  // �Ķ��� �ʹ� ������ ���� Ȯ�� ����

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

            // �����丮�� �߰� (��ź�� ����)
            AddToHistory(moleIndex, elapsedTime);
        }

        // �δ��� ����
        GameObject mole = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

        // Ÿ�� ���� (prefab�� �̹� �����Ǿ� �ִٸ� �� �κ� ���� ����)
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

        // ���� ���� ���� ����
        holeOccupied[randomIndex] = true;
        StartCoroutine(ReleaseHoleAfterDelay(randomIndex, 1.0f));

        // �δ��� �ڵ� ����
        Destroy(mole, 1.0f);
    }

    void AddToHistory(int moleType, float currentTime)
    {
        // �Ϲ� �δ����� �����丮�� �߰� (0: Red, 1: Blue)
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
        // historyWindow �ð����� ������ ��� ����
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
                break; // ť�� �ð������� ���ĵǾ� �����Ƿ� �� �̻� Ȯ���� �ʿ� ����
            }
        }
    }

    IEnumerator ReleaseHoleAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        holeOccupied[index] = false;
    }
}