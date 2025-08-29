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

        // Firebase �ʱ�ȭ Ȯ��
        if (db == null) Debug.LogError("Firestore �ʱ�ȭ ����");
        if (auth == null) Debug.LogError("Auth �ʱ�ȭ ����");
    }

    public async void LoadRankingForGame(string gameId)
    {
        currentGameId = gameId;
        Debug.Log($"��ŷ �ε� ����: {gameId}");

        try
        {
            // ������ ���: gameScores/{gameId}/records
            var query = db.Collection("gameScores").Document(gameId)
                          .Collection("records")
                          .OrderByDescending("score") // �ʿ� �� �ִ� ��� ������ ascending����
                          .Limit(50);

            Debug.Log($"���� ���: gameScores/{gameId}/records");

            var snap = await query.GetSnapshotAsync();
            Debug.Log($"������ ������: {snap.Count}�� ����");

            if (snap.Count == 0)
            {
                Debug.LogWarning($"���� {gameId}�� ���� ����� �����ϴ�.");
                // UI �ʱ�ȭ
                foreach (Transform child in rankingListContent) Destroy(child.gameObject);
                if (myRankText != null)
                {
                    myRankText.text = "��� ����";
                    myRankText.color = Color.gray;
                }
                return;
            }

            var rankingList = new List<RankingData>();
            int rank = 1;

            foreach (var doc in snap.Documents)
            {
                Debug.Log($"���� ID: {doc.Id}");

                string name = doc.ContainsField("nickname") ? doc.GetValue<string>("nickname") : "Unknown";
                float score = doc.GetValue<float>("score");
                bool isMine = doc.Id == auth.CurrentUser?.UserId;

                Debug.Log($"���� {rank}: {name}, ����: {score}, �� ���: {isMine}");

                rankingList.Add(new RankingData(rank++, name, score, isMine));
            }

            UpdateRankingList(rankingList);
            UpdateMyRank(rankingList);
        }
        catch (Exception e)
        {
            Debug.LogError($"��ŷ �ε� ����: {e.Message}");
            Debug.LogError($"���� Ʈ���̽�: {e.StackTrace}");

            if (myRankText != null)
            {
                myRankText.text = "�ε� ����";
                myRankText.color = Color.red;
            }
        }
    }

    void UpdateRankingList(List<RankingData> rankingList)
    {
        // ���� �����۵� ����
        foreach (Transform child in rankingListContent)
        {
            Destroy(child.gameObject);
        }

        // ���ο� ��ŷ �����۵� ����
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
                Debug.LogWarning("RankingItem ������Ʈ�� ã�� �� �����ϴ�.");
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
                Debug.LogWarning("����ڰ� �α��εǾ� ���� �ʽ��ϴ�.");
                return;
            }

            string uid = auth.CurrentUser.UserId;

            // ������ ���: gameScores/{gameId}/records/{uid}
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
                Debug.Log($"���� ��� ������Ʈ �Ϸ�! ����: {gameId}, ����: {score}");

                // ���� ������ ��ŷ�̸� �ٽ� �ε�
                if (gameId == currentGameId)
                {
                    LoadRankingForGame(gameId);
                }
            }
            else
            {
                Debug.Log($"���� ����({oldScore})�� �� ���Ƽ� ������Ʈ���� �ʽ��ϴ�. �� ����: {score}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"���� ���� ����: {e.Message}");
            Debug.LogError($"���� Ʈ���̽�: {e.StackTrace}");
        }
    }
}