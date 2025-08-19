using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Knife_KnifeUI : MonoBehaviour
{
    public GameObject knifeIconPrefab; // Į ������ ������

    public Sprite knifeIcon;
    public Sprite emptyIcon;

    public Transform knifePanel;       // Į �����ܵ��� �� �θ� (UI Panel)

    private List<Image> knifeIcons = new List<Image>();
    private int usedKnives = 0;

    // Į ���� ����
    public void SetKnives(int count)
    {
        Debug.Log($"SetKnives called with count: {count}");
        // ���� UI ����
        foreach (var icon in knifeIcons)
        {
            Destroy(icon.gameObject);
        }
        knifeIcons.Clear();

        usedKnives = 0;

        // ���� ����
        for (int i = 0; i < count; i++)
        {
            GameObject icon = Instantiate(knifeIconPrefab, knifePanel);
            Image image = icon.GetComponent<Image>();
            image.sprite = knifeIcon;
            knifeIcons.Add(image);
        }
    }

    // Į �ϳ� ����
    public void UseKnife()
    {
        if (knifeIcons.Count > 0)
        {
            knifeIcons[usedKnives].sprite = emptyIcon;
            usedKnives++;
        }
    }
}
