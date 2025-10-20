using UnityEngine;

public class DogeBtnLeft : MonoBehaviour
{
    public DogePlayer player;           // 이동할 대상
    public float speed = 3f;            // 이동 속도
    public Vector2 moveDirection = Vector2.left;  // 왼쪽으로 이동

    void Update()
    {
        Vector2 inputPos = Vector2.zero;
        bool clicked = false;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clicked = true;
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            inputPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            clicked = true;
        }
#endif

        if (clicked)
        {
            // 클릭 위치에 Collider2D가 있는지 확인
            Collider2D hit = Physics2D.OverlapPoint(inputPos);
            if (hit != null && hit.gameObject == gameObject && player != null)
            {
                // 한 번 클릭 시 왼쪽으로 이동
                player.transform.Translate(moveDirection * speed * Time.deltaTime);
            }
        }
    }
}
