using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[System.Serializable]
public class ScoreData
{
    public string player;   // 닉네임
    public float record;    // 점수나 기록
}

[System.Serializable]
public class GameRankingData
{
    public string gameName;
    public bool ascendingOrder; // true = 낮은 기록 우선, false = 높은 기록 우선
    public List<ScoreData> records = new List<ScoreData>();

    public GameRankingData(string name, bool ascending)
    {
        gameName = name;
        ascendingOrder = ascending;
        records = new List<ScoreData>();
    }
}

public class FirebaseRanking : MonoBehaviour
{
    public RankingPopup rankingPopup;

    string baseUrl = "https://ampersand-jjangmyeon-default-rtdb.asia-southeast1.firebasedatabase.app/";

    public static FirebaseRanking Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 이미 존재하면 새 오브젝트 파괴
        }
    }

    // 점수 업로드
    public void UploadScore(string gameName, string playerName, float record)
    {
        ScoreData data = new ScoreData { player = playerName, record = record };
        string json = JsonUtility.ToJson(data);
        StartCoroutine(PostRequest(gameName, json));
    }

    IEnumerator PostRequest(string gameName, string json)
    {
        string url = $"{baseUrl}/{gameName}/scores.json";
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Score uploaded!");
        else
            Debug.LogError(request.error);
    }

    // 랭킹 불러오기
    public void GetTopScores(string gameName, bool ascending = false)
    {
        StartCoroutine(GetRequest(gameName, ascending));
    }

    IEnumerator GetRequest(string gameName, bool ascending)
    {
        string url = $"{baseUrl}/{gameName}/scores.json";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log($"Received data: {json}");

            // MiniJSON 필요
            var dict = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;

            GameRankingData rankingData = new GameRankingData(gameName, ascending);

            if (dict != null)
            {
                foreach (var kvp in dict)
                {
                    var entry = kvp.Value as Dictionary<string, object>;
                    rankingData.records.Add(new ScoreData
                    {
                        player = entry["player"].ToString(),
                        record = float.Parse(entry["record"].ToString())
                    });
                }
            }

            // UI에 전달
            rankingPopup.UpdateGameRanking(gameName, rankingData);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}
