using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform rankingListContent;    // 세로 ScrollView Content (랭킹 리스트)
    public GameObject rankingItemPrefab;    // 랭킹 리스트 아이템 Prefab
    public TMP_Text myRankText;            // 내 순위 표시용 Text
    public Button refreshButton;            // 새로고침 버튼

    private string currentGameId;
    private Dictionary<string, List<RankingData>> cachedRankingData = new Dictionary<string, List<RankingData>>();
    private GameButtonManager gameButtonManager;

    // 랭킹 데이터 클래스
    [System.Serializable]
    public class RankingData
    {
        public int rank;
        public string playerName;
        public int score;
        public DateTime playTime;
        public bool isMyRecord; // 내 기록인지 여부

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
        // GameButtonManager 참조 가져오기
        gameButtonManager = FindObjectOfType<GameButtonManager>();

        if (gameButtonManager != null)
        {
            // 게임 선택 이벤트 구독
            gameButtonManager.OnGameSelected += LoadRankingForGame;
        }
        else
        {
            Debug.LogError("GameButtonManager를 찾을 수 없습니다!");
        }
    }

    void InitializeUI()
    {
        // 새로고침 버튼 이벤트 연결
        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(() => RefreshCurrentRanking());
        }
    }

    // GameButtonManager에서 호출되는 함수
    public void LoadRankingForGame(string gameId)
    {
        currentGameId = gameId;
        StartCoroutine(LoadRankingCoroutine(gameId));
    }

    IEnumerator LoadRankingCoroutine(string gameId)
    {
        // 캐시된 데이터가 있는지 확인
        List<RankingData> rankingDataList;

        if (cachedRankingData.ContainsKey(gameId))
        {
            rankingDataList = cachedRankingData[gameId];
        }
        else
        {
            // TODO: Firebase에서 데이터 가져오기
            // 임시로 더미 데이터 사용
            yield return new WaitForSeconds(0.1f); // 짧은 딜레이

            rankingDataList = GenerateDummyRankingData(gameId);
            cachedRankingData[gameId] = rankingDataList;
        }

        // 랭킹 리스트 업데이트
        UpdateRankingList(rankingDataList);

        // 내 순위 업데이트
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
        // 내 기록 찾기
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
            // 캐시 삭제하고 다시 로드
            if (cachedRankingData.ContainsKey(currentGameId))
            {
                cachedRankingData.Remove(currentGameId);
            }

            LoadRankingForGame(currentGameId);
        }
    }

    // 더미 데이터 생성 (나중에 Firebase 연동시 제거)
    List<RankingData> GenerateDummyRankingData(string gameId)
    {
        List<RankingData> list = new List<RankingData>();

        // 플레이어 이름 풀
        string[] playerNames = { "김민수", "이지영", "박철수", "최영희", "정우진",
                                "한소영", "오세훈", "임다영", "윤상호", "장미라",
                                "강동원", "송혜교", "조인성", "전지현", "유재석" };

        for (int i = 1; i <= 20; i++)
        {
            bool isMyRecord = (i == UnityEngine.Random.Range(5, 15)); // 랜덤하게 내 기록 설정

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

    // 외부에서 새로운 기록 추가시 호출할 함수
    public void AddNewRecord(string gameId, string playerName, int score)
    {
        // TODO: Firebase에 새 기록 추가
        Debug.Log($"새 기록 추가: {gameId} - {playerName}: {score}점");

        // 캐시 삭제 (다음에 새로 로드하도록)
        if (cachedRankingData.ContainsKey(gameId))
        {
            cachedRankingData.Remove(gameId);
        }

        // 현재 게임이면 즉시 새로고침
        if (currentGameId == gameId)
        {
            LoadRankingForGame(gameId);
        }
    }

    // 특정 게임의 랭킹으로 바로 이동
    public void ShowGameRanking(string gameId)
    {
        if (gameButtonManager != null)
        {
            gameButtonManager.SelectGame(gameId);
        }
    }
}