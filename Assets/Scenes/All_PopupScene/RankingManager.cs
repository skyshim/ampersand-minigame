using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform rankingListContent;    // ���� ScrollView Content (��ŷ ����Ʈ)
    public GameObject rankingItemPrefab;    // ��ŷ ����Ʈ ������ Prefab
    public TMP_Text myRankText;            // �� ���� ǥ�ÿ� Text
    public Button refreshButton;            // ���ΰ�ħ ��ư

    private string currentGameId;
    private Dictionary<string, List<RankingData>> cachedRankingData = new Dictionary<string, List<RankingData>>();
    private GameButtonManager gameButtonManager;

    // ��ŷ ������ Ŭ����
    [System.Serializable]
    public class RankingData
    {
        public int rank;
        public string playerName;
        public int score;
        public DateTime playTime;
        public bool isMyRecord; // �� ������� ����

        public RankingData(int rank, string playerName, int score, DateTime playTime = default, bool isMyRecord = false)
        {
            this.rank = rank;
            this.playerName = playerName;
            this.score = score;
            this.playTime = playTime == default ? DateTime.Now : playTime;
            this.isMyRecord = isMyRecord;
        }
    }

    void Start()
    {
        InitializeComponents();
        InitializeUI();
    }

    void InitializeComponents()
    {
        // GameButtonManager ���� ��������
        gameButtonManager = FindObjectOfType<GameButtonManager>();

        if (gameButtonManager != null)
        {
            // ���� ���� �̺�Ʈ ����
            gameButtonManager.OnGameSelected += LoadRankingForGame;
        }
        else
        {
            Debug.LogError("GameButtonManager�� ã�� �� �����ϴ�!");
        }
    }

    void InitializeUI()
    {
        // ���ΰ�ħ ��ư �̺�Ʈ ����
        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(() => RefreshCurrentRanking());
        }
    }

    // GameButtonManager���� ȣ��Ǵ� �Լ�
    public void LoadRankingForGame(string gameId)
    {
        currentGameId = gameId;
        StartCoroutine(LoadRankingCoroutine(gameId));
    }

    IEnumerator LoadRankingCoroutine(string gameId)
    {
        // ĳ�õ� �����Ͱ� �ִ��� Ȯ��
        List<RankingData> rankingDataList;

        if (cachedRankingData.ContainsKey(gameId))
        {
            rankingDataList = cachedRankingData[gameId];
        }
        else
        {
            // TODO: Firebase���� ������ ��������
            // �ӽ÷� ���� ������ ���
            yield return new WaitForSeconds(0.1f); // ª�� ������

            rankingDataList = GenerateDummyRankingData(gameId);
            cachedRankingData[gameId] = rankingDataList;
        }

        // ��ŷ ����Ʈ ������Ʈ
        UpdateRankingList(rankingDataList);

        // �� ���� ������Ʈ
        UpdateMyRank(rankingDataList);
    }

    void UpdateRankingList(List<RankingData> rankingDataList)
    {
        ClearRankingList();

        foreach (var data in rankingDataList)
        {
            GameObject item = Instantiate(rankingItemPrefab, rankingListContent);
            RankingItem rankingItem = item.GetComponent<RankingItem>();

            if (rankingItem != null)
            {
                rankingItem.SetData(data);
            }
        }
    }

    void UpdateMyRank(List<RankingData> rankingDataList)
    {
        // �� ��� ã��
        RankingData myRecord = rankingDataList.Find(data => data.isMyRecord);

        if (myRecord != null && myRankText != null)
        {
            myRankText.text = myRecord.rank.ToString();
            myRankText.color = Color.yellow;
        }
    }

    void ClearRankingList()
    {
        foreach (Transform child in rankingListContent)
        {
            Destroy(child.gameObject);
        }
    }

    void RefreshCurrentRanking()
    {
        if (!string.IsNullOrEmpty(currentGameId))
        {
            // ĳ�� �����ϰ� �ٽ� �ε�
            if (cachedRankingData.ContainsKey(currentGameId))
            {
                cachedRankingData.Remove(currentGameId);
            }

            LoadRankingForGame(currentGameId);
        }
    }

    // ���� ������ ���� (���߿� Firebase ������ ����)
    List<RankingData> GenerateDummyRankingData(string gameId)
    {
        List<RankingData> list = new List<RankingData>();

        // �÷��̾� �̸� Ǯ
        string[] playerNames = { "��μ�", "������", "��ö��", "�ֿ���", "������",
                                "�Ѽҿ�", "������", "�Ӵٿ�", "����ȣ", "��̶�",
                                "������", "������", "���μ�", "������", "���缮" };

        for (int i = 1; i <= 20; i++)
        {
            bool isMyRecord = (i == UnityEngine.Random.Range(5, 15)); // �����ϰ� �� ��� ����

            list.Add(new RankingData(
                rank: i,
                playerName: playerNames[UnityEngine.Random.Range(0, playerNames.Length)],
                score: UnityEngine.Random.Range(10000 - (i * 300), 10000 - (i * 200)),
                playTime: DateTime.Now.AddDays(-UnityEngine.Random.Range(0, 30)),
                isMyRecord: isMyRecord
            ));
        }

        return list;
    }

    // �ܺο��� ���ο� ��� �߰��� ȣ���� �Լ�
    public void AddNewRecord(string gameId, string playerName, int score)
    {
        // TODO: Firebase�� �� ��� �߰�
        Debug.Log($"�� ��� �߰�: {gameId} - {playerName}: {score}��");

        // ĳ�� ���� (������ ���� �ε��ϵ���)
        if (cachedRankingData.ContainsKey(gameId))
        {
            cachedRankingData.Remove(gameId);
        }

        // ���� �����̸� ��� ���ΰ�ħ
        if (currentGameId == gameId)
        {
            LoadRankingForGame(gameId);
        }
    }

    // Ư�� ������ ��ŷ���� �ٷ� �̵�
    public void ShowGameRanking(string gameId)
    {
        if (gameButtonManager != null)
        {
            gameButtonManager.SelectGame(gameId);
        }
    }
}