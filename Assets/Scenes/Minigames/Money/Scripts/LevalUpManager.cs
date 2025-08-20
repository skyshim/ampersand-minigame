using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    [Header("Refs")]
    public Money money;                       // Money 컴포넌트 참조
    public TextMeshProUGUI levelText;         // 레벨업 창: "1LV"
    public TextMeshProUGUI clickText;         // "클릭 당 돈 : 1,000"
    public TextMeshProUGUI nextCostText;      // "다음 레벨 : 10,000₩"
    public TextMeshProUGUI mainpanelLvText;   // 메인: "케릭터 레벨 : X"
    public Button levelUpButton;

    [Header("Stats")]
    public int level = 1;                     // 현재 레벨(1~100)
    public int clickValue = 1000;             // 현재 클릭당 돈
    public int defaultIncreasePerLevel = 1000;// 기본 증가량(티어/수동 없을 때)

    [Header("Level Limit")]
    public int maxLevel = 100;                // 만렙

    [Header("Costs (직접 지정)")]
    [Tooltip("costs[L] = L레벨로 올릴 때 드는 비용. 인덱스 2~100만 사용.")]
    public int[] costs = new int[101];        // 0,1은 미사용. 2~100 채우기

    [System.Serializable]
    public class ClickTier
    {
        public int startLevel = 1;            // 이 레벨 이상부터 적용
        public int addPerLevel = 1000;        // 해당 구간의 1레벨업당 증가량
        public float mulPerLevel = 1f;        // 배수 보정(선택)
    }

    [Header("Increase Rule (둘 다 사용 가능)")]
    [Tooltip("manualIncrease[L] = L레벨로 오를 때 사용할 '직접 지정 증가량'. 0이면 무시.")]
    public int[] manualIncrease = new int[101]; // 2~100만 의미 있음(0=미지정)
    [Tooltip("티어 규칙: 가장 큰 startLevel을 만족하는 항목 적용")]
    public ClickTier[] tiers;                   // 구간별 증가량 테이블

    void OnEnable()
    {
        if (money != null) money.OnMoneyChanged += _ => RefreshUI();
        RefreshUI();
    }

    void OnDisable()
    {
        if (money != null) money.OnMoneyChanged -= _ => RefreshUI();
    }

    void Start()
    {
        if (levelUpButton != null)
            levelUpButton.onClick.AddListener(TryLevelUp);
    }

    public void TryLevelUp()
    {
        if (level >= maxLevel) return;

        int cost = CurrentCost();
        if (cost <= 0) return; // 비용 미설정/0이면 무시

        // 조건문: 돈 충분 → 소비 → 레벨업 처리
        if (money != null && money.Spend(cost))
        {
            int nextLevel = level + 1;

            // 증가량 계산 (수동 > 티어 > 기본)
            int inc = CalcIncreaseFor(nextLevel);
            clickValue += Mathf.Max(0, inc);

            // 실제 클릭 수익도 갱신
            money.SetMoneyValue(clickValue);

            // 레벨 상승
            level = nextLevel;

            RefreshUI();
        }
        else
        {
            // 돈 부족 피드백(텍스트 깜빡임/사운드 등) 넣고 싶으면 여기
        }
    }

    void RefreshUI()
    {
        if (levelText != null) levelText.text = $"{level}LV";
        if (mainpanelLvText != null) mainpanelLvText.text = $"케릭터 레벨 : {level}";
        if (clickText != null) clickText.text = $"클릭 당 돈 : {clickValue:N0}";

        if (level >= maxLevel)
        {
            if (nextCostText != null) nextCostText.text = "다음 레벨 : MAX";
            if (levelUpButton != null) levelUpButton.interactable = false;
        }
        else
        {
            int cost = CurrentCost();
            if (nextCostText != null) nextCostText.text = $"다음 레벨 : {cost:N0}₩";

            if (levelUpButton != null)
                levelUpButton.interactable = (money != null && money.GetMoney() + 1e-6f >= cost && cost > 0);
        }
    }

    // 레벨 L→(L+1) 비용
    int CurrentCost()
    {
        int next = level + 1;
        if (next > maxLevel) return 0;
        if (costs == null || costs.Length <= next) return 0;
        return Mathf.Max(0, costs[next]);
    }

    // (우선순위) manualIncrease > tiers > defaultIncreasePerLevel
    int CalcIncreaseFor(int newLevel)
    {
        // 1) 수동 지정 우선
        if (manualIncrease != null && manualIncrease.Length > newLevel)
        {
            int manual = manualIncrease[newLevel];
            if (manual != 0) return manual;
        }

        // 2) 티어 규칙(가장 높은 startLevel 만족 항목 적용)
        ClickTier best = null;
        if (tiers != null)
        {
            foreach (var t in tiers)
            {
                if (t == null) continue;
                if (newLevel >= t.startLevel && (best == null || t.startLevel > best.startLevel))
                    best = t;
            }
        }
        if (best != null)
        {
            return Mathf.RoundToInt(best.addPerLevel * Mathf.Max(0f, best.mulPerLevel));
        }

        // 3) 기본 증가량
        return Mathf.Max(0, defaultIncreasePerLevel);
    }
}
