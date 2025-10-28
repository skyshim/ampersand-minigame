using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPopup : MonoBehaviour
{
    [SerializeField] private Transform rankContent;
    [SerializeField] private GameObject rankItemPrefab;
    [SerializeField] private Transform gameButtonParent;

    private Dictionary<string, GameRankingData> allGames = new Dictionary<string, GameRankingData>();

    void Start()
    {
        FirebaseRanking.Instance.rankingPopup = this;
        allGames.Add("칼던지기", new GameRankingData("칼던지기", false));
        allGames.Add("똥피하기", new GameRankingData("똥피하기", false));
        allGames.Add("수박게임", new GameRankingData("수박게임", false));
        allGames.Add("1to50", new GameRankingData("1to50", true));
        InitializeButtons();
    }

    void InitializeButtons()
    {
        foreach (Transform btn in gameButtonParent)
        {
            string gameName = btn.GetComponentInChildren<Text>().text;
            btn.GetComponent<Button>().onClick.AddListener(() => OnGameSelected(gameName));
        }
    }

    void OnGameSelected(string gameName)
    {
        if (!allGames.ContainsKey(gameName)) return;
        FirebaseRanking.Instance.GetTopScores(gameName, allGames[gameName].ascendingOrder);
        PopulateRankingList(allGames[gameName]);
    }

    public void PopulateRankingList(GameRankingData data)
    {
        foreach (Transform child in rankContent)
            Destroy(child.gameObject);

        // 정렬
        if (data.ascendingOrder)
            data.records.Sort((a, b) => a.record.CompareTo(b.record));
        else
            data.records.Sort((a, b) => b.record.CompareTo(a.record));

        // UI 생성
        for (int i = 0; i < data.records.Count; i++)
        {
            var record = data.records[i];
            GameObject item = Instantiate(rankItemPrefab, rankContent);
            Text[] texts = item.GetComponentsInChildren<Text>();
            texts[0].text = (i + 1).ToString();      // 순위
            texts[1].text = record.player;           // 닉네임
            texts[2].text = record.record.ToString(); // 기록
        }
    }

    public void UpdateGameRanking(string gameName, GameRankingData rankingData)
    {
        if (allGames.ContainsKey(gameName))
            allGames[gameName] = rankingData;
        else
            allGames.Add(gameName, rankingData);
    }
}
