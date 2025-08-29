using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class AutoSaveManager : MonoBehaviour
    {
        [Header("References")]
        public LevelUpManager levelUpManager; // LevelUpManager 컴포넌트를 가진 오브젝트
        public Money money;                   // Money 컴포넌트 (돈 보유)

        [Header("Settings")]
        [Tooltip("자동 저장 주기(초). 120초 = 2분")]
        public float intervalSeconds = 120f;

        private bool _running = false;

        void OnEnable()
        {
            // 레퍼런스 자동 탐색(비어있으면 찾아봄)
            if (levelUpManager == null) levelUpManager = FindObjectOfType<LevelUpManager>();
            if (money == null) money = FindObjectOfType<Money>();

            _running = true;
            StartCoroutine(AutoSaveLoop());
        }

        void OnDisable()
        {
            _running = false;
            // 비활성화될 때 한 번 더 저장(선택)
            TrySaveNow();
        }

        System.Collections.IEnumerator AutoSaveLoop()
        {
            // 첫 저장(선택) — 원치 않으면 주석
            TrySaveNow();

            var wait = new WaitForSecondsRealtime(Mathf.Max(5f, intervalSeconds)); // 최소 5초 가드
            while (_running)
            {
                yield return wait;
                TrySaveNow();
            }
        }

        private void TrySaveNow()
        {
            if (levelUpManager != null && money != null)
            {
                SaveSystem.Save(levelUpManager, money);
#if UNITY_EDITOR
                Debug.Log($"[AutoSaveManager] Saved at {System.DateTime.Now:HH:mm:ss}");
#endif
            }
        }

        // 앱이 백그라운드로 갈 때(모바일) 즉시 저장
        void OnApplicationPause(bool pause)
        {
            if (pause) TrySaveNow();
        }

        // 앱 종료 시 즉시 저장
        void OnApplicationQuit()
        {
            TrySaveNow();
        }
    }
}
