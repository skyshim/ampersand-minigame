using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public Transform topBarContent;  // TopBar ScrollView Content (가로)
    public GameObject gameButtonPrefab;  // 게임 선택용 버튼 Prefab

    public Transform rankingListContent;  // 세로 ScrollView Content (랭킹 리스트)
    public GameObject rankingItemPrefab;  // 랭킹 리스트 아이템 Prefab

    public Text myRankText;  // 내 순위 표시용 Text

    private List<string> gameNames = new List<string>() { "Game1", "Game2", "Game3", /*...*/};
    private string currentGame;

    void Start()
    {
        CreateGameButtons();
        // 기본 첫 게임 랭킹 로드
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
        // TODO: 랭킹 데이터 가져와서 리스트 갱신하기
        Debug.Log($"랭킹 불러오기: {gameName}");

        ClearRankingList();

        // 샘플 더미 데이터 (나중에 DB 연동)
        List<RankingData> rankingDataList = GetDummyRankingData(gameName);

        // 랭킹 리스트 생성
        foreach (var data in rankingDataList)
        {
            GameObject item = Instantiate(rankingItemPrefab, rankingListContent);
            item.GetComponent<RankingItem>().SetData(data);
        }

        // 내 순위도 표시 (예: 7위)
        myRankText.text = $"내 순위: {UnityEngine.Random.Range(1, 100)}위";
    }

    void ClearRankingList()
    {
        foreach (Transform child in rankingListContent)
        {
            Destroy(child.gameObject);
        }
    }

    // 임시 더미 랭킹 데이터 클래스와 생성 함수
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
