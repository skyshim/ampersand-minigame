using TMPro;
using UnityEngine;

public class Volley_PlayerController : MonoBehaviour
{
    public int playerId = 1; // 1 = ����, 2 = ������
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    private bool moveLeft, moveRight, actionPressed;

    public LayerMask ballLayer;
    public float spikeForce = 12f;
    public Transform hitPoint; // �� ��ġ ���� Transform

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
        // Player1: ���� ����
        if (playerId == 1)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Min(pos.x, -0.1f); // 0 �̻� �� �Ѿ
            transform.position = pos;
        }
        // Player2: ������ ����
        else if (playerId == 2)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Max(pos.x, 0.1f); // 0 ���� �� �Ѿ
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
        else if (playerId == 2) // Player 2 (��,��,��,��)
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
            if (isGrounded) actionButtonText.text = "��";   // ����
            else actionButtonText.text = "* ";             // ������ũ
        }

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
    }

    void HandleTouchInput()
    {
        if (Input.touchCount <= 0) return;

        foreach (Touch touch in Input.touches)
        {
            // ����/������ ȭ�� ������
            bool isMySide = (playerId == 1 && touch.position.x < Screen.width / 2) ||
                            (playerId == 2 && touch.position.x >= Screen.width / 2);

            if (!isMySide) continue;

            if (touch.phase == TouchPhase.Began)
            {
                // �����ϰ�: �� = ����
                Jump();
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // �巡�� �������� �̵�
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
                // "�� �� ��" ���� ����
                Vector2 dir = (rb.position - (Vector2)hitPoint.position).normalized;

                // ���� �ӵ� �ʱ�ȭ
                rb.velocity = Vector2.zero;

                // �ش� �������� ���ϰ� �б�
                float spikePower = 8f; // �� ũ�� (Ʃ�� ����)
                rb.AddForce(dir * spikePower, ForceMode2D.Impulse);

                Debug.Log($"Spike! ���� = {dir}, �� = {spikePower}");
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
