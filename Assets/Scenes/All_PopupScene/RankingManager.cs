using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public Transform topBarContent;  // TopBar ScrollView Content (����)
    public GameObject gameButtonPrefab;  // ���� ���ÿ� ��ư Prefab

    public Transform rankingListContent;  // ���� ScrollView Content (��ŷ ����Ʈ)
    public GameObject rankingItemPrefab;  // ��ŷ ����Ʈ ������ Prefab

    public Text myRankText;  // �� ���� ǥ�ÿ� Text

    private List<string> gameNames = new List<string>() { "Game1", "Game2", "Game3", /*...*/};
    private string currentGame;

    void Start()
    {
        CreateGameButtons();
        // �⺻ ù ���� ��ŷ �ε�
        if (gameNames.Count > 0)
            LoadRanking(gameNames[0]);
    }

    void CreateGameButtons()
    {
        foreach (string gameName in gameNames)
        {
            GameObject btnObj = Instantiate(gameButtonPrefab, topBarContent);
            btnObj.GetComponentInChildren<Text>().text = gameName;

            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() => OnGameButtonClicked(gameName));
        }
    }

    void OnGameButtonClicked(string gameName)
    {
        currentGame = gameName;
        LoadRanking(gameName);
    }

    void LoadRanking(string gameName)
    {
        // TODO: ��ŷ ������ �����ͼ� ����Ʈ �����ϱ�
        Debug.Log($"��ŷ �ҷ�����: {gameName}");

        ClearRankingList();

        // ���� ���� ������ (���߿� DB ����)
        List<RankingData> rankingDataList = GetDummyRankingData(gameName);

        // ��ŷ ����Ʈ ����
        foreach (var data in rankingDataList)
        {
            GameObject item = Instantiate(rankingItemPrefab, rankingListContent);
            item.GetComponent<RankingItem>().SetData(data);
        }

        // �� ������ ǥ�� (��: 7��)
        myRankText.text = $"�� ����: {UnityEngine.Random.Range(1, 100)}��";
    }

    void ClearRankingList()
    {
        foreach (Transform child in rankingListContent)
        {
            Destroy(child.gameObject);
        }
    }

    // �ӽ� ���� ��ŷ ������ Ŭ������ ���� �Լ�
    public class RankingData
    {
        public int rank;
        public string playerName;
        public int score;
    }

    List<RankingData> GetDummyRankingData(string gameName)
    {
        List<RankingData> list = new List<RankingData>();
        for (int i = 1; i <= 10; i++)
        {
            list.Add(new RankingData()
            {
                rank = i,
                playerName = $"{gameName}_Player{i}",
                score = UnityEngine.Random.Range(1000, 5000)
            });
        }
        return list;
    }
}
