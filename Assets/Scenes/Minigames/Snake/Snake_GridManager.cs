using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_GridManager : MonoBehaviour
{
    public GameObject tilePrefab; // 타일 프리팹
    public int gridCount = 9;     // 9x9 그리드
    private GameObject gridParent;



    public void SetupCamera()
    {
        // 맵 전체 높이를 화면에 맞춤
        float screenRatio = (float)Screen.width / Screen.height;
        Camera.main.orthographicSize = (gridCount / screenRatio) / 2f;
    }

    public void CreateGrid()
    {
        gridParent = new GameObject("Grid");

        float half = gridCount / 2f;

        for (int x = 0; x < gridCount; x++)
        {
            for (int y = 0; y < gridCount; y++)
            {
                Vector2 pos = new Vector2(x - half + 0.5f, y - half + 0.5f);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);
                tile.transform.parent = gridParent.transform;
                tile.transform.localScale = Vector3.one;

                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if ((x + y) % 2 == 0) sr.color = Color.white;
                else sr.color = new Color(0.9f, 0.9f, 0.9f);
            }
        }
    }
}
