using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KnifeUI : MonoBehaviour
{
    public GameObject knifeIconPrefab; // 칼 아이콘 프리팹
    public Transform knifePanel;       // 칼 아이콘들이 들어갈 부모 (UI Panel)

    private List<GameObject> knifeIcons = new List<GameObject>();

    // 칼 개수 설정
    public void SetKnives(int count)
    {
        Debug.Log($"SetKnives called with count: {count}");
        // 기존 UI 제거
        foreach (var icon in knifeIcons)
        {
            Destroy(icon);
        }
        knifeIcons.Clear();

        // 새로 생성
        for (int i = 0; i < count; i++)
        {
            GameObject icon = Instantiate(knifeIconPrefab, knifePanel);
            knifeIcons.Add(icon);
        }
    }

    // 칼 하나 제거
    public void UseKnife()
    {
        if (knifeIcons.Count > 0)
        {
            GameObject last = knifeIcons[knifeIcons.Count - 1];
            knifeIcons.RemoveAt(knifeIcons.Count - 1);
            Destroy(last);
        }
    }
}
