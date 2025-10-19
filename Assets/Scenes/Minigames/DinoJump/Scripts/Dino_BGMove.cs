using UnityEngine;

public class Dino_BGMove : MonoBehaviour
{
    public float speed = 2f;         // 배경이 왼쪽으로 움직이는 속도
    private RectTransform rectTransform;
    private float width;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        width = rectTransform.rect.width;
    }

    void Update()
    {
        // 왼쪽으로 이동
        rectTransform.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        // 화면 밖으로 완전히 나가면 오른쪽으로 재배치
        if (rectTransform.anchoredPosition.x <= -width)
        {
            rectTransform.anchoredPosition += new Vector2(width * 2f, 0);
        }
    }
}
