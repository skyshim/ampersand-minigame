using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_FoodManager : MonoBehaviour
{
    public GameObject foodPrefab;
    public int gridCount = 9;

    private GameObject currentFood;
    private Snake_Move snake;

    void Start()
    {
        snake = FindObjectOfType<Snake_Move>();
    }

    public void SpawnFood()
    {
        if (currentFood != null) Destroy(currentFood);

        float half = gridCount / 2f;
        Vector2 pos;

        // Snake 몸 위치와 겹치지 않도록 반복
        do
        {
            int x = Random.Range(0, gridCount);
            int y = Random.Range(0, gridCount);
            pos = new Vector2(x - half + 0.5f, y - half + 0.5f);
        }
        while (IsOnSnake(pos));

        currentFood = Instantiate(foodPrefab, pos, Quaternion.identity);
    }

    bool IsOnSnake(Vector2 pos)
    {
        foreach (Transform part in snake.bodyParts)
        {
            if ((Vector2)part.position == pos)
                return true;
        }
        return false;
    }

    public Vector2 GetFoodPosition()
    {
        return currentFood != null ? (Vector2)currentFood.transform.position : Vector2.zero;
    }
}
