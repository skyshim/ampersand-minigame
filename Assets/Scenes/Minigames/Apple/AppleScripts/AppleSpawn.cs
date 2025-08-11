using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject applePrefab;
    [SerializeField]
    private GameObject appleParent;

    private readonly int apple_width = 17, apple_height = 10;
    private readonly int apple_spacing = 20;

    private void Awake()
    {
        SpawnApple();
    }

    public void SpawnApple()
    {
        if (applePrefab == null || appleParent == null)
        {
            Debug.LogError("ApplePrefab or AppleParent is not assigned in the Inspector!");
            return;
        }

        Vector2 apple_size = applePrefab.GetComponent<RectTransform>().sizeDelta;
        apple_size += new Vector2(apple_spacing, apple_spacing);

        for (int apple_y = 0; apple_y < apple_height; ++apple_y)
        {
            for (int apple_x = 0; apple_x < apple_width; ++apple_x)
            {
                GameObject apple_clone = Instantiate(applePrefab, appleParent.transform);
                RectTransform apple_rect = apple_clone.GetComponent<RectTransform>();

                float apple_px = (-apple_width * 0.5f + 0.5f + apple_x) * apple_size.x;
                float apple_py = (-apple_height * 0.5f + 0.5f + apple_y) * apple_size.y;
                apple_rect.anchoredPosition = new Vector2(apple_px, apple_py);
            }
        }
    }
}