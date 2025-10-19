using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Snake_Move : MonoBehaviour
{
    public GameObject bodyPrefab;
    public Snake_FoodManager foodManager;
    public float moveInterval = 0.3f;
    public int gridCount = 9;
    public int score = 0;

    private Vector2Int direction = Vector2Int.right;
    public List<Transform> bodyParts = new List<Transform>();
    private float timer = 0f;

    void Start()
    {
        bodyParts.Add(transform);  // 머리
        transform.position = Vector3.zero;

        // 초기 몸통 4칸 생성 (머리 뒤쪽)
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos = transform.position - new Vector3(i + 1, 0, 0); // 오른쪽에서 왼쪽으로
            GameObject newPart = Instantiate(bodyPrefab, pos, Quaternion.identity);
            bodyParts.Add(newPart.transform);
        }
        foodManager.SpawnFood();
    }

    void Update()
    {
        HandleInput();

        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            Move();
            timer = 0f;
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2Int.down) direction = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2Int.up) direction = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2Int.right) direction = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2Int.left) direction = Vector2Int.right;
    }

    void Move()
    {
        Vector3 nextPos = bodyParts[0].position + new Vector3(direction.x, direction.y, 0);

        float half = gridCount / 2f;
        if (nextPos.x < -half || nextPos.x > half || nextPos.y < -half || nextPos.y > half)
        {
            Debug.Log("Game Over! Hit Wall");
            enabled = false;
            return;
        }

        foreach (Transform part in bodyParts)
        {
            if (part.position == nextPos)
            {
                Debug.Log("Game Over! Hit Self");
                enabled = false;
                return;
            }
        }

        for (int i = bodyParts.Count - 1; i > 0; i--)
        {
            bodyParts[i].position = bodyParts[i - 1].position; 
        }

        bodyParts[0].position = nextPos;

        if ((Vector2)nextPos == foodManager.GetFoodPosition())
        {
            Grow();
            foodManager.SpawnFood();
        }
    }

    public void Grow()
    {
        GameObject newPart = Instantiate(bodyPrefab, bodyParts[bodyParts.Count - 1].position, Quaternion.identity);
        bodyParts.Add(newPart.transform);
        score++;
    }
}
