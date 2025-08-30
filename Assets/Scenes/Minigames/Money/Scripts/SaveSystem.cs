using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // PlayerPrefs 키
    private const string K_LEVEL = "PLAYER_LEVEL";
    private const string K_CLICKVALUE = "PLAYER_CLICKVALUE";
    private const string K_MONEY = "PLAYER_MONEY";


    // 저장
    public static void Save(LevelUpManager lu, Money money)
        {
            if (lu == null || money == null) return;

            PlayerPrefs.SetInt(K_LEVEL, (int)lu.level);
            // clickValue는 ulong → 문자열로 저장(정밀도 보존)
            PlayerPrefs.SetString(K_CLICKVALUE, lu.clickValue.ToString());
            PlayerPrefs.SetFloat(K_MONEY, money.GetMoney());
            PlayerPrefs.Save();
#if UNITY_EDITOR
            Debug.Log("[SaveSystem] Saved.");
#endif
        }

        // 불러오기 (키가 없으면 현재 값 유지)
        public static void Load(LevelUpManager lu, Money money)
        {
            if (lu == null || money == null) return;

            if (PlayerPrefs.HasKey(K_LEVEL))
                lu.level = (ulong)Mathf.Max(1, PlayerPrefs.GetInt(K_LEVEL));

            if (PlayerPrefs.HasKey(K_CLICKVALUE))
            {
                var s = PlayerPrefs.GetString(K_CLICKVALUE, lu.clickValue.ToString());
                if (ulong.TryParse(s, out var cv)) lu.clickValue = cv;
            }

            if (PlayerPrefs.HasKey(K_MONEY))
                money.SetMoneyAbsolute(PlayerPrefs.GetFloat(K_MONEY));

            // Money가 사용하는 클릭 파워 업데이트
            money.SetMoneyValue((float)lu.clickValue);
#if UNITY_EDITOR
            Debug.Log("[SaveSystem] Loaded.");
#endif
        }
        // 전체 초기화 (프로그레스 리셋)
        public static void ResetAll(LevelUpManager lu, Money money,
                                    ulong resetLevel = 1, ulong resetClickValue = 1000, float resetMoney = 0f)
        {
            if (lu == null || money == null) return;

            // 저장데이터 삭제
            PlayerPrefs.DeleteKey(K_LEVEL);
            PlayerPrefs.DeleteKey(K_CLICKVALUE);
            PlayerPrefs.DeleteKey(K_MONEY);
            PlayerPrefs.Save();

            // 런타임 상태도 초기화
            lu.level = resetLevel;
            lu.clickValue = resetClickValue;
            money.SetMoneyAbsolute(resetMoney);
            money.SetMoneyValue((float)lu.clickValue);
#if UNITY_EDITOR
            Debug.Log("[SaveSystem] ResetAll done.");
#endif
        }
    }
