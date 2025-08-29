using TMPro;
using UnityEngine;

public class Volley_PlayerController : MonoBehaviour
{
    public int playerId = 1; // 1 = 왼쪽, 2 = 오른쪽
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    private bool moveLeft, moveRight, actionPressed;

    public LayerMask ballLayer;
    public float spikeForce = 12f;
    public Transform hitPoint; // 손 위치 같은 Transform

    public TMP_Text actionButtonText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleKeyboardInput();
        HandleTouchInput();
    }

    void LateUpdate()
    {
        // Player1: 왼쪽 영역
        if (playerId == 1)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Min(pos.x, -0.1f); // 0 이상 못 넘어감
            transform.position = pos;
        }
        // Player2: 오른쪽 영역
        else if (playerId == 2)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Max(pos.x, 0.1f); // 0 이하 못 넘어감
            transform.position = pos;
        }
    }

    void HandleKeyboardInput()
    {
        float move = 0f;

        if (playerId == 1) // Player 1 (A,D,W,S)
        {
            if (Input.GetKey(KeyCode.A)) move = -1f;
            if (Input.GetKey(KeyCode.D)) move = 1f;

            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
                Jump();

            if (Input.GetKeyDown(KeyCode.S))
                Spike();
        }
        else if (playerId == 2) // Player 2 (←,→,↑,↓)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) move = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) move = 1f;

            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
                Jump();

            if (Input.GetKeyDown(KeyCode.DownArrow))
                Spike();
        }

        if (moveLeft) move = -1;
        if (moveRight) move = 1;
        if (actionPressed)
        {
            if (isGrounded) Jump();
            else Spike();
            actionPressed = false;
        }

        if (actionButtonText != null)
        {
            if (isGrounded) actionButtonText.text = "↑";   // 점프
            else actionButtonText.text = "* ";             // 스파이크
        }

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
    }

    void HandleTouchInput()
    {
        if (Input.touchCount <= 0) return;

        foreach (Touch touch in Input.touches)
        {
            // 왼쪽/오른쪽 화면 나누기
            bool isMySide = (playerId == 1 && touch.position.x < Screen.width / 2) ||
                            (playerId == 2 && touch.position.x >= Screen.width / 2);

            if (!isMySide) continue;

            if (touch.phase == TouchPhase.Began)
            {
                // 간단하게: 탭 = 점프
                Jump();
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // 드래그 방향으로 이동
                float dir = touch.deltaPosition.x;
                rb.velocity = new Vector2(Mathf.Sign(dir) * moveSpeed, rb.velocity.y);
            }
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    void Spike()
    {
        Debug.Log($"Player {playerId} Spike!");
        Collider2D ball = Physics2D.OverlapCircle(hitPoint.position, 1f, ballLayer);
        if (ball != null)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // "손 → 공" 벡터 방향
                Vector2 dir = (rb.position - (Vector2)hitPoint.position).normalized;

                // 기존 속도 초기화
                rb.velocity = Vector2.zero;

                // 해당 방향으로 강하게 밀기
                float spikePower = 8f; // 힘 크기 (튜닝 가능)
                rb.AddForce(dir * spikePower, ForceMode2D.Impulse);

                Debug.Log($"Spike! 방향 = {dir}, 힘 = {spikePower}");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }


    public void OnMoveLeftDown() { moveLeft = true; }
    public void OnMoveLeftUp() { moveLeft = false; }
    public void OnMoveRightDown() { moveRight = true; }
    public void OnMoveRightUp() { moveRight = false; }
    public void OnActionButton() { actionPressed = true; }
}
