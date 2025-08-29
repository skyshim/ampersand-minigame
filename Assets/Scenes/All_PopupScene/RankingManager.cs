using System;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;
using TMPro;

public class RankingManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform rankingListContent;
    public GameObject rankingItemPrefab;
    public TMP_Text myRankText;
    public GameObject rankingPopup;

    private string currentGameId;
    private FirebaseFirestore db;
    private FirebaseAuth auth;

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
    }

    public async void LoadRankingForGame(string gameId)
    {
        currentGameId = gameId;
        Debug.Log($"랭킹 로드 시작: {gameId}");

        try
        {
            // 수정된 경로: gameScores/{gameId}/records
            var query = db.Collection("gameScores").Document(gameId)
                          .Collection("records")
                          .OrderByDescending("score") // 필요 시 최단 기록 게임은 ascending으로
                          .Limit(50);

            Debug.Log($"쿼리 경로: gameScores/{gameId}/records");

            var snap = await query.GetSnapshotAsync();
            Debug.Log($"스냅샷 가져옴: {snap.Count}개 문서");

            if (snap.Count == 0)
            {
                Debug.LogWarning($"게임 {gameId}에 점수 기록이 없습니다.");
                // UI 초기화
                foreach (Transform child in rankingListContent) Destroy(child.gameObject);
                if (myRankText != null)
                {
                    myRankText.text = "기록 없음";
                    myRankText.color = Color.gray;
                }
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

            if (myRankText != null)
            {
                myRankText.text = "로드 실패";
                myRankText.color = Color.red;
            }
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
        if (myRankText != null)
        {
            if (myRecord != null)
            {
                myRankText.text = $"{myRecord.rank}";
                myRankText.color = Color.yellow;
            }
            else
            {
                myRankText.text = "?";
                myRankText.color = Color.gray;
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
                    { "displayName", playerName },
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