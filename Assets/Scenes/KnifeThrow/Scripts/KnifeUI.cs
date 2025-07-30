using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KnifeUI : MonoBehaviour
{
    public GameObject knifeIconPrefab; // Į ������ ������
    public Transform knifePanel;       // Į �����ܵ��� �� �θ� (UI Panel)

    private List<GameObject> knifeIcons = new List<GameObject>();

    // Į ���� ����
    public void SetKnives(int count)
    {
        Debug.Log($"SetKnives called with count: {count}");
        // ���� UI ����
        foreach (var icon in knifeIcons)
        {
            Destroy(icon);
        }
        knifeIcons.Clear();

        // ���� ����
        for (int i = 0; i < count; i++)
        {
            GameObject icon = Instantiate(knifeIconPrefab, knifePanel);
            knifeIcons.Add(icon);
        }
    }

    // Į �ϳ� ����
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
