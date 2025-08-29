using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelUpManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject levelUpPanel;
    public GameObject CharacterPanel;
    public GameObject SettingPanel;
    public GameObject ChallengePanel;
    public GameObject resetConfirmPanel;

    [Header("Refs")]
    public Money money;                       // Money 컴포넌트(돈/클릭 파워 보유, float 기반)
    public TextMeshProUGUI levelText;         // 레벨업 창: "1LV"
    public TextMeshProUGUI clickText;         // "클릭 당 돈 : 1,000"
    public TextMeshProUGUI nextCostText;      // "다음 레벨 : 10,000₩"
    public TextMeshProUGUI mainpanelLvText;   // 메인: "케릭터 레벨 : X"
    public Button levelUpButton;

    [Header("Stats")]
    public ulong level = 1;                       // 현재 레벨
    public ulong clickValue = 1000;               // 현재 클릭당 돈(LV1 기준)
    public ulong defaultIncreasePerLevel = 1000;  // (티어/수동 없을 때) 기본 증가량

    [Header("Level Limit")]
    public ulong maxLevel = 70;                   // 만렙 (필요 시 변경)

    [Header("Costs (직접 지정)")]
    [Tooltip("costs[L] = L레벨로 올릴 때 드는 비용. 인덱스 2..maxLevel 사용. (0,1 미사용)")]
    public ulong[] costs = new ulong[71];         // 기본 1~70 대응

    [System.Serializable]
    public class ClickTier
    {
        public ulong startLevel = 1;              // 이 레벨 이상부터 적용
        public ulong addPerLevel = 1000;          // 해당 구간의 1레벨업당 증가량
        public float mulPerLevel = 1f;            // 배수(등차 느낌이면 1)
    }

    [Header("Increase Rule (둘 다 사용 가능)")]
    [Tooltip("manualIncrease[L] = L레벨로 오를 때 사용할 '직접 지정 증가량'. 0이면 무시(우선순위 1위).")]
    public ulong[] manualIncrease = new ulong[101]; // 2.. 사용, 0=미지정
    [Tooltip("티어 규칙: 가장 큰 startLevel을 만족하는 항목 적용(우선순위 2위).")]
    public ClickTier[] tiers;                      // 등차 원하면 Mul=1으로

    // ===== Auto Tuning (레벨업당 목표 클릭 수) =====
    [Header("Auto Tuning (Clicks per level-up)")]
    public int targetClicksMin = 10;            // 초반 목표 클릭 수(예: 10)
    public int targetClicksMax = 1000;          // 후반 목표 클릭 수(예: 1000)
    public bool useGeometricRamp = true;        // true=기하식, false=선형
    public ulong minIncreasePerLevel = 1;       // 레벨업 최소 증가량 보장
    public bool roundToThousands = false;       // 증가량을 1,000 단위 반올림

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

        ulong cost = CurrentCost();
        if (cost == 0) return; // 비용 미설정/0이면 무시

        // Money는 float 기반 → 캐스팅
        if (money != null && money.Spend((float)cost))
        {
            ulong nextLevel = level + 1;

            // 증가량(수동 > 티어 > 기본)
            ulong inc = CalcIncreaseFor(nextLevel);
            clickValue += inc;

            // 실제 클릭 수익도 갱신
            money.SetMoneyValue((float)clickValue);

            level = nextLevel;

            RefreshUI();
        }
        else
        {
            // 돈 부족 피드백(원하면: 텍스트 깜빡임/사운드)
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
            ulong cost = CurrentCost();
            if (nextCostText != null) nextCostText.text = $"다음 레벨 : {cost:N0}₩";

            if (levelUpButton != null)
            {
                double have = money != null ? (double)money.GetMoney() : 0.0;
                levelUpButton.interactable = (have + 1e-6 >= (double)cost && cost > 0);
            }
        }
    }

    // 레벨 L → (L+1) 비용
    ulong CurrentCost()
    {
        ulong next = level + 1;
        if (next > maxLevel) return 0;
        if (costs == null || costs.Length <= (int)next) return 0;
        return costs[next];
    }

    // (우선순위) manualIncrease > tiers > defaultIncreasePerLevel
    ulong CalcIncreaseFor(ulong newLevel)
    {
        // 1) 수동 지정
        if (manualIncrease != null && manualIncrease.Length > (int)newLevel)
        {
            ulong manual = manualIncrease[newLevel];
            if (manual != 0) return manual;
        }

        // 2) 티어 규칙 (가장 높은 startLevel)
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
            float mul = Mathf.Max(0f, best.mulPerLevel); // 등차→1
            return (ulong)Mathf.RoundToInt((float)best.addPerLevel * mul);
        }

        // 3) 기본 증가량
        return defaultIncreasePerLevel;
    }

    // ===== 에디터 툴: manualIncrease 자동 생성 / 프리뷰 =====
#if UNITY_EDITOR
    [ContextMenu("Auto-Generate ManualIncrease (Clicks 10→1000)")]
    void AutoGenerateManualIncrease()
    {
        int needLen = (int)maxLevel + 1;
        if (costs == null || costs.Length < needLen)
        {
            Debug.LogError($"[AutoGenerateManualIncrease] costs 길이가 부족합니다. (need >= {needLen})");
            return;
        }
        if (manualIncrease == null || manualIncrease.Length < needLen)
            manualIncrease = new ulong[needLen];

        double power = System.Math.Max(1.0, (double)clickValue); // LV1 클릭 파워

        int steps = (int)maxLevel - 1; // 1→2 .. (max-1)→max
        double kMin = System.Math.Max(1.0, (double)targetClicksMin);
        double kMax = System.Math.Max(kMin, (double)targetClicksMax);

        double TargetK(int i)
        {
            // i: 1..steps
            double t = (double)(i - 1) / System.Math.Max(1, steps - 1);
            if (useGeometricRamp)
            {
                // 기하 증가(자연스럽게 점진)
                return kMin * System.Math.Pow(kMax / kMin, t);
            }
            else
            {
                // 선형 증가
                return kMin + t * (kMax - kMin);
            }
        }

        // 초기화
        for (int i = 0; i < manualIncrease.Length; i++) manualIncrease[i] = 0UL;

        var sb = new StringBuilder();
        sb.AppendLine("== Auto ManualIncrease Generated ==");
        sb.AppendLine($"Start power(LV1): {power:N0}");
        sb.AppendLine($"Target clicks: {kMin:N0} → {kMax:N0} ({(useGeometricRamp ? "geometric" : "linear")})");

        for (int i = 1; i <= steps; i++)
        {
            int fromLv = i;       // 1..(max-1)
            int toLv = i + 1;   // 2..max

            ulong cost = costs[toLv];
            double k = TargetK(i);                      // 목표 클릭 수
            double requiredPower = (k > 0.0) ? (double)cost / k : (double)cost;

            double delta = requiredPower - power;       // 이번 레벨업 증가량
            if (delta < (double)minIncreasePerLevel) delta = (double)minIncreasePerLevel;

            double rounded = System.Math.Round(delta);
            if (roundToThousands) rounded = System.Math.Floor(rounded / 1000.0) * 1000.0;

            // ulong 범위 보호
            ulong inc = (ulong)System.Math.Max(0.0, System.Math.Min((double)ulong.MaxValue, rounded));
            manualIncrease[toLv] = inc;

            power += inc; // 다음 구간 대비 파워 누적

            sb.AppendLine($"LV {fromLv} -> {toLv} | cost: {cost:N0} | target clicks: {k:F1} | inc: {inc:N0} | newPower: {power:N0}");
        }

        if (tiers != null && tiers.Length > 0)
            Debug.LogWarning("지금 자동 튜닝은 manualIncrease만 사용합니다. 등차 느낌이면 tiers의 Mul=1 또는 비활성 권장.");

        Debug.Log(sb.ToString());
        EditorUtility.SetDirty(this);
    }

    [ContextMenu("Preview Clicks Needed (with current arrays)")]
    void PreviewClicksNeeded()
    {
        int needLen = (int)maxLevel + 1;
        if (costs == null || costs.Length < needLen)
        {
            Debug.LogError($"[PreviewClicksNeeded] costs 길이가 부족합니다. (need >= {needLen})");
            return;
        }

        double power = System.Math.Max(1.0, (double)clickValue);
        var sb = new StringBuilder();
        sb.AppendLine("== Preview: Clicks needed per level-up ==");
        sb.AppendLine($"LV 1 power: {power:N0}");

        for (int lv = 1; lv < (int)maxLevel; lv++)
        {
            ulong cost = costs[lv + 1];
            double clicks = (power > 0.0) ? (double)cost / power : (double)cost;
            sb.AppendLine($"LV {lv} -> {lv + 1} | cost: {cost:N0} | power: {power:N0} | clicks ≈ {clicks:N1}");

            // 다음 레벨에서의 파워 갱신(수동 증가 우선)
            ulong inc = 0UL;
            if (manualIncrease != null && manualIncrease.Length > lv + 1)
                inc = manualIncrease[lv + 1];
            power += inc;
        }
        Debug.Log(sb.ToString());
    }

    public void SaveProgress()
    {
        SaveSystem.Save(this, money);
    }

    // 수동 불러오기 버튼에 연결
    public void LoadProgress()
    {
        SaveSystem.Load(this, money);
        RefreshUI();
    }

    // 전체 초기화 버튼에 연결
    public void ResetAllProgress()
    {
        // 초기값은 프로젝트에 맞게 조절(기본: 1레벨, 클릭 1000, 돈 0)
        SaveSystem.ResetAll(this, money, resetLevel: 1, resetClickValue: 1000, resetMoney: 0f);
        RefreshUI();
    }

    // 앱 종료 시 자동 저장(선택)
    void OnApplicationQuit()
    {
        SaveSystem.Save(this, money);
    }


    //reset 패널 열기(아마 UImanager로 할듯)
    //public void OpenResetConfirm() {
    //    if (resetConfirmPanel != null) resetConfirmPanel.SetActive(true);
    //}
    public void CloseResetConfirm() {
        mainPanel.SetActive(true);
        levelUpPanel.SetActive(false);
        CharacterPanel.SetActive(false);
        SettingPanel.SetActive(false);
        ChallengePanel.SetActive(false);
        resetConfirmPanel.SetActive(false);
}
    public void ConfirmResetAndClose() {
        ResetAllProgress();
        CloseResetConfirm();
    }
#endif
}
