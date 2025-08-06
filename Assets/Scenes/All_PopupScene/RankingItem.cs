using UnityEngine;
using UnityEngine.UI;

public class RankingItem : MonoBehaviour
{
    public Text rankText;
    public Text playerNameText;
    public Text scoreText;

    public void SetData(RankingManager.RankingData data)
    {
        rankText.text = data.rank.ToString();
        playerNameText.text = data.playerName;
        scoreText.text = data.score.ToString();
    }
}
