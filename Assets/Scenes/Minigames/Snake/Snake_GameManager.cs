using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_GameManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = -4; x <= 4; x++)
        {
            for (int y = -4; y <= 4; y++)
            {
                var tile = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                tile.transform.parent = gridParent.transform;
            }
        }

    }
}
