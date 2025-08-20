using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Knife_KnifeUI : MonoBehaviour
{
    public GameObject knifeIconPrefab; // 칼 아이콘 프리팹

    public Sprite knifeIcon;
    public Sprite emptyIcon;

    public Transform knifePanel;       // 칼 아이콘들이 들어갈 부모 (UI Panel)

    private List<Image> knifeIcons = new List<Image>();
    private int usedKnives = 0;

    // 칼 개수 설정
    public void SetKnives(int count)
    {
        Debug.Log($"SetKnives called with count: {count}");
        // 기존 UI 제거
        foreach (var icon in knifeIcons)
        {
            Destroy(icon.gameObject);
        }
        knifeIcons.Clear();

        usedKnives = 0;

        // 새로 생성
        for (int i = 0; i < count; i++)
        {
            GameObject icon = Instantiate(knifeIconPrefab, knifePanel);
            Image image = icon.GetComponent<Image>();
            image.sprite = knifeIcon;
            knifeIcons.Add(image);
        }
    }

    // 칼 하나 제거
    public void UseKnife()
    {
        if (knifeIcons.Count > 0)
        {
            knifeIcons[usedKnives].sprite = emptyIcon;
            usedKnives++;
        }
    }
}
