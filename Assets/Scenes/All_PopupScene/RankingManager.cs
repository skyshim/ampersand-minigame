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

        // Firebase �ʱ�ȭ Ȯ��
        if (db == null) Debug.LogError("Firestore �ʱ�ȭ ����");
        if (auth == null) Debug.LogError("Auth �ʱ�ȭ ����");

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseRankingPopup);
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //  �� �̵��ص� ����
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
        Debug.Log($"��ŷ �ε� ����: {gameId}");

        try
        {
            // ������ ���: gameScores/{gameId}/records
            var collectionRef = db.Collection("gameScores").Document(gameId).Collection("records");
            var query = isLowestBetter
                ? collectionRef.OrderBy("score").Limit(50)           // ��������
                : collectionRef.OrderByDescending("score").Limit(50); // ��������

            Debug.Log($"���� ���: gameScores/{gameId}/records");

            var snap = await query.GetSnapshotAsync();
            Debug.Log($"������ ������: {snap.Count}�� ����");

            if (snap.Count == 0)
            {
                Debug.LogWarning($"���� {gameId}�� ���� ����� �����ϴ�.");
                // UI �ʱ�ȭ
                foreach (Transform child in rankingListContent) Destroy(child.gameObject);
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

        if (myRankingItem != null)
        {
            var myRankingComp = myRankingItem.GetComponent<RankingItem>();
            if (myRankingComp != null)
            {
                if (myRecord != null)
                {
                    // �� ��ŷ ������ ����
                    myRankingComp.SetData(myRecord);
                }
                else
                {
                    // ����� ���� ��� �ʱ�ȭ
                    myRankingComp.rankText.text = "-";
                    myRankingComp.playerNameText.text = "��� ����";
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
                    { "nickname", playerName },
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