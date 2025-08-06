using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawner : MonoBehaviour
{
    public GameObject[] molePrefabs; // 0: ���� �δ���, 1: �Ķ� �δ���, 2: ���� ��ź, 3: �Ķ� ��ź
    public Transform[] spawnPoints;

    private bool[] holeOccupied;
    private Coroutine spawnCoroutine;

    private Queue<int> recentMoleHistory = new Queue<int>(); // 0=Red, 1=Blue
    private int recentRedCount = 0;
    private int recentBlueCount = 0;
    [SerializeField] private float historyWindow = 5f; // �� �ʰ� ��� ����

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
        int moleIndex;

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

        float bombChance = Mathf.Clamp01(elapsedTime / 40f * 0.5f);

        GameObject selectedPrefab;
        if (Random.value < bombChance)
        {
            moleIndex = Random.Range(2, 4);
            selectedPrefab = molePrefabs[moleIndex];
        }
        else
        {
            // �δ��� �� ����: ����ġ ������ ����
            float baseWeight = 1f;
            float redWeight = baseWeight;
            float blueWeight = baseWeight;

            int diff = recentRedCount - recentBlueCount;

            if (diff > 2) blueWeight += 0.5f;   // ������ �ʹ� ������ �Ķ� Ȯ�� �ø�
            else if (diff < -2) redWeight += 0.5f;

            float totalWeight = redWeight + blueWeight;
            float rand = Random.Range(0f, totalWeight);

            if (rand < redWeight) moleIndex = 0;
            else moleIndex = 1;

            selectedPrefab = molePrefabs[moleIndex];
        }

        Mole moleScript = selectedPrefab.GetComponent<Mole>();

        // molePrefabs ����: 0:RedMole, 1:BlueMole, 2:RedBomb, 3:BlueBomb
        switch (moleIndex)
        {
            case 0: moleScript.moleType = MoleType.RedMole; break;
            case 1: moleScript.moleType = MoleType.BlueMole; break;
            case 2: moleScript.moleType = MoleType.RedBomb; break;
            case 3: moleScript.moleType = MoleType.BlueBomb; break;
        }

        GameObject mole = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

        holeOccupied[randomIndex] = true;
        StartCoroutine(ReleaseHoleAfterDelay(randomIndex, 1.0f));
        Destroy(mole, 1.0f);
    }

    IEnumerator ReleaseHoleAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        holeOccupied[index] = false;
    }




    void RecordMole(int moleIndex)
    {
        if (moleIndex == 0) // ���� �δ���
            recentRedCount++;
        else if (moleIndex == 1) // �Ķ� �δ���
            recentBlueCount++;

        recentMoleHistory.Enqueue(moleIndex);

        // ��� ������ �ڷ�ƾ ����
        StartCoroutine(RemoveMoleAfterDelay(historyWindow));
    }
    IEnumerator RemoveMoleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (recentMoleHistory.Count > 0)
        {
            int oldIndex = recentMoleHistory.Dequeue();
            if (oldIndex == 0) recentRedCount--;
            else if (oldIndex == 1) recentBlueCount--;
        }
    }
}