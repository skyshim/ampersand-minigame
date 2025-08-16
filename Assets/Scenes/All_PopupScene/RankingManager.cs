using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Auth;

public class RankingManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform rankingListContent;
    public GameObject rankingItemPrefab;
    public TMP_Text myRankText;
    public Button refreshButton;
    public Button closeButton;
    public GameObject rankingPopup;

    private string currentGameId;
    private Dictionary<string, List<RankingData>> cachedRankingData = new Dictionary<string, List<RankingData>>();
    private GameButtonManager gameButtonManager;

    private FirebaseFirestore db;
    private FirebaseAuth auth;

    [System.Serializable]
    public class RankingData
    {
        public int rank;
        public string playerName;
        public int score;
        public DateTime playTime;
        public bool isMyRecord;


        public RankingData(int rank, string playerName, int score, DateTime playTime = default, bool isMyRecord = false)
        {
            this.rank = rank;
            this.playerName = playerName;
            this.score = score;
            this.playTime = playTime == default ? DateTime.Now : playTime;
            this.isMyRecord = isMyRecord;
        }
    }

    void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
    }

    void Start()
    {
        InitializeComponents();
        InitializeUI();

        closeButton.onClick.AddListener(ClosePopup);
    }

    void InitializeComponents()
    {
        gameButtonManager = FindObjectOfType<GameButtonManager>();
        if (gameButtonManager != null)
        {
            gameButtonManager.OnGameSelected += LoadRankingForGame;
        }
        else
        {
            Debug.LogError("GameButtonManager를 찾을 수 없습니다!");
        }
    }

    void InitializeUI()
    {
        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(() => RefreshCurrentRanking());
        }
    }

    public void LoadRankingForGame(string gameId)
    {
        currentGameId = gameId;
        StartCoroutine(LoadRankingCoroutine(gameId));
    }

    IEnumerator LoadRankingCoroutine(string gameId)
    {
        if (cachedRankingData.ContainsKey(gameId))
        {
            var cachedList = cachedRankingData[gameId];
            UpdateRankingList(cachedList);
            UpdateMyRank(cachedList);
            yield break;
        }

        // Firebase에서 데이터 가져오기
        var rankingDataList = new List<RankingData>();
        Query query = db.Collection("Games").Document(gameId)
                        .Collection("Scores")
                        .OrderByDescending("score") // 게임별 기준에 따라 변경 가능
                        .Limit(50);

        var task = query.GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Firebase 데이터 로드 실패: " + task.Exception);
            yield break;
        }

        int rank = 1;
        foreach (var doc in task.Result.Documents)
        {
            string name = doc.ContainsField("displayName") ? doc.GetValue<string>("displayName") : "Unknown";
            float scoreFloat = doc.GetValue<float>("score");
            bool isMine = doc.Id == auth.CurrentUser.UserId;

            rankingDataList.Add(new RankingData(rank++, name, Mathf.RoundToInt(scoreFloat), DateTime.Now, isMine));
        }

        cachedRankingData[gameId] = rankingDataList;

        // UI 업데이트는 메인 스레드에서
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            UpdateRankingList(rankingDataList);
            UpdateMyRank(rankingDataList);
        });
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
        RankingData myRecord = rankingDataList.Find(data => data.isMyRecord);
        if (myRecord != null && myRankText != null)
        {
            myRankText.text = myRecord.rank.ToString();
            myRankText.color = Color.gray;
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
            if (cachedRankingData.ContainsKey(currentGameId))
            {
                cachedRankingData.Remove(currentGameId);
            }
            LoadRankingForGame(currentGameId);
        }
    }

    public void AddNewRecord(string gameId, string playerName, float score)
    {
        string uid = auth.CurrentUser.UserId;
        DocumentReference docRef = db.Collection("Games").Document(gameId)
                                     .Collection("Scores").Document(uid);

        docRef.GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("점수 조회 실패: " + task.Exception);
                return;
            }

            float oldScore = task.Result.Exists ? task.Result.GetValue<float>("score") : float.MinValue;
            if (scoreIsBetter(score, oldScore))
            {
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "displayName", playerName },
                    { "score", score },
                    { "timestamp", Timestamp.GetCurrentTimestamp() }
                };

                docRef.SetAsync(data).ContinueWith(setTask =>
                {
                    if (setTask.IsCompleted)
                    {
                        // 캐시 삭제 후 새로 로드
                        if (cachedRankingData.ContainsKey(gameId))
                            cachedRankingData.Remove(gameId);

                        if (currentGameId == gameId)
                            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                                LoadRankingForGame(gameId));
                    }
                });
            }
        });
    }

    bool scoreIsBetter(float newScore, float oldScore)
    {
        // 게임별 점수 기준 적용 (예시)
        if (currentGameId == "TimeAttackGame") return newScore < oldScore; // 빠른 시간일수록 좋음
        return newScore > oldScore; // 점수 경쟁형
    }

    public void ShowGameRanking(string gameId)
    {
        if (gameButtonManager != null)
        {
            gameButtonManager.SelectGame(gameId);
        }
    }

    void ClosePopup()
    {
        rankingPopup.SetActive(false);
    }
}
