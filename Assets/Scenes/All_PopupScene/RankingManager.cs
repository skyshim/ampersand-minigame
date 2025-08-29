using System;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }

    [Header("UI References")]
    public Transform rankingListContent;
    public GameObject rankingItemPrefab;
    public GameObject myRankingItem;
    public GameObject rankingPopup;
    public Button closeButton;

    private string currentGameId;
    private FirebaseFirestore db;
    public FirebaseAuth auth;

    [System.Serializable]
    public class RankingData
    {
        

        public int rank;
        public string playerName;
        public float score;
        public bool isMyRecord;
        public RankingData(int rank, string playerName, float score, bool isMyRecord)
        {
            this.rank = rank;
            this.playerName = playerName;
            this.score = score;
            this.isMyRecord = isMyRecord;
        }
    }

    void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        // Firebase 초기화 확인
        if (db == null) Debug.LogError("Firestore 초기화 실패");
        if (auth == null) Debug.LogError("Auth 초기화 실패");

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseRankingPopup);
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //  씬 이동해도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CloseRankingPopup()
    {
        if (rankingPopup != null)
        {
            rankingPopup.SetActive(false);
        }
    }

    public async void LoadRankingForGame(string gameId, bool isLowestBetter = false)
    {
        currentGameId = gameId;
        Debug.Log($"랭킹 로드 시작: {gameId}");

        try
        {
            // 수정된 경로: gameScores/{gameId}/records
            var collectionRef = db.Collection("gameScores").Document(gameId).Collection("records");
            var query = isLowestBetter
                ? collectionRef.OrderBy("score").Limit(50)           // 오름차순
                : collectionRef.OrderByDescending("score").Limit(50); // 내림차순

            Debug.Log($"쿼리 경로: gameScores/{gameId}/records");

            var snap = await query.GetSnapshotAsync();
            Debug.Log($"스냅샷 가져옴: {snap.Count}개 문서");

            if (snap.Count == 0)
            {
                Debug.LogWarning($"게임 {gameId}에 점수 기록이 없습니다.");
                // UI 초기화
                foreach (Transform child in rankingListContent) Destroy(child.gameObject);
                return;
            }

            var rankingList = new List<RankingData>();
            int rank = 1;

            foreach (var doc in snap.Documents)
            {
                Debug.Log($"문서 ID: {doc.Id}");

                string name = doc.ContainsField("nickname") ? doc.GetValue<string>("nickname") : "Unknown";
                float score = doc.GetValue<float>("score");
                bool isMine = doc.Id == auth.CurrentUser?.UserId;

                Debug.Log($"순위 {rank}: {name}, 점수: {score}, 내 기록: {isMine}");

                rankingList.Add(new RankingData(rank++, name, score, isMine));
            }

            UpdateRankingList(rankingList);
            UpdateMyRank(rankingList);
        }
        catch (Exception e)
        {
            Debug.LogError($"랭킹 로드 실패: {e.Message}");
            Debug.LogError($"스택 트레이스: {e.StackTrace}");
        }
    }

    void UpdateRankingList(List<RankingData> rankingList)
    {
        // 기존 아이템들 삭제
        foreach (Transform child in rankingListContent)
        {
            Destroy(child.gameObject);
        }

        // 새로운 랭킹 아이템들 생성
        foreach (var data in rankingList)
        {
            GameObject item = Instantiate(rankingItemPrefab, rankingListContent);
            RankingItem ri = item.GetComponent<RankingItem>();
            if (ri != null)
            {
                ri.SetData(data);
            }
            else
            {
                Debug.LogWarning("RankingItem 컴포넌트를 찾을 수 없습니다.");
            }
        }
    }

    void UpdateMyRank(List<RankingData> rankingList)
    {
        var myRecord = rankingList.Find(r => r.isMyRecord);

        if (myRankingItem != null)
        {
            var myRankingComp = myRankingItem.GetComponent<RankingItem>();
            if (myRankingComp != null)
            {
                if (myRecord != null)
                {
                    // 내 랭킹 데이터 적용
                    myRankingComp.SetData(myRecord);
                }
                else
                {
                    // 기록이 없을 경우 초기화
                    myRankingComp.rankText.text = "-";
                    myRankingComp.playerNameText.text = "기록 없음";
                    myRankingComp.scoreText.text = "-";
                }
            }
        }
    }

    public async void AddNewRecord(string gameId, string playerName, float score, bool isLowestBetter = false)
    {
        try
        {
            if (auth.CurrentUser == null)
            {
                Debug.LogWarning("사용자가 로그인되어 있지 않습니다.");
                return;
            }

            string uid = auth.CurrentUser.UserId;

            // 수정된 경로: gameScores/{gameId}/records/{uid}
            var docRef = db.Collection("gameScores").Document(gameId)
                           .Collection("records").Document(uid);

            var snap = await docRef.GetSnapshotAsync();
            float oldScore = snap.Exists ? snap.GetValue<float>("score") : (isLowestBetter ? float.MaxValue : float.MinValue);

            bool better = isLowestBetter ? score < oldScore : score > oldScore;
            if (better || !snap.Exists)
            {
                var data = new Dictionary<string, object>
                {
                    { "nickname", playerName },
                    { "score", score },
                    { "timestamp", Timestamp.GetCurrentTimestamp() }
                };
                await docRef.SetAsync(data);
                Debug.Log($"점수 기록 업데이트 완료! 게임: {gameId}, 점수: {score}");

                // 현재 게임의 랭킹이면 다시 로드
                if (gameId == currentGameId)
                {
                    LoadRankingForGame(gameId);
                }
            }
            else
            {
                Debug.Log($"기존 점수({oldScore})가 더 좋아서 업데이트하지 않습니다. 새 점수: {score}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"점수 저장 실패: {e.Message}");
            Debug.LogError($"스택 트레이스: {e.StackTrace}");
        }
    }
}