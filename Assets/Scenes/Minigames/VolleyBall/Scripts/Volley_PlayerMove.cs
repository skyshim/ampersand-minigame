using System.Collections;
using TMPro;
using UnityEngine;

public class Volley_PlayerMove : MonoBehaviour
{
    public int playerId = 1; // 1 = 왼쪽, 2 = 오른쪽
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    private bool moveLeft, moveRight, actionPressed;

    public LayerMask ballLayer;
    public float spikeForce = 8f;
    public float spikeRadius = 1.5f;
    public Transform hitPoint; // 손 위치 같은 Transform

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleKeyboardInput();
        HandleActionButton();
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

        if (moveLeft) move = -1;
        if (moveRight) move = 1;

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
    }

    public bool GroundCheck()
    {
        return isGrounded;
    }

    void HandleActionButton()
    {
        if (actionPressed)
        {
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                TrySpike();
            }
        }
    }

    void TrySpike()
    {
        // 범위 내 공 찾기
        Collider2D ball = Physics2D.OverlapCircle(hitPoint.position, spikeRadius, ballLayer);

        if (ball != null)
        {
            Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                PerformSpike(ballRb);
            }
        }
    }

    void PerformSpike(Rigidbody2D ballRb)
    {
        // 스파이크 방향 계산
        Vector2 spikeDirection = (ballRb.transform.position - transform.position).normalized;

        // 공에 강한 힘 가하기
        ballRb.velocity = Vector2.zero; // 기존 속도 리셋
        ballRb.AddForce(spikeDirection * spikeForce, ForceMode2D.Impulse);

        // 잠시 물리 재질 변경으로 강한 반발력 추가
        StartCoroutine(ApplySpikeBounce(ballRb));
    }

    private IEnumerator ApplySpikeBounce(Rigidbody2D ballRb)
    {
        Collider2D ballCollider = ballRb.GetComponent<Collider2D>();
        if (ballCollider == null) yield break;

        PhysicsMaterial2D originalMat = ballCollider.sharedMaterial;

        // 임시 물리 재질 생성
        PhysicsMaterial2D spikeMat = new PhysicsMaterial2D("SpikeMaterial");
        spikeMat.bounciness = 0.9f;
        spikeMat.friction = 0.1f;

        ballCollider.sharedMaterial = spikeMat;

        // 0.2초 후 원래 재질로 복구
        yield return new WaitForSeconds(0.2f);

        ballCollider.sharedMaterial = originalMat;

        // 임시 재질 정리
        if (spikeMat != null)
        {
            DestroyImmediate(spikeMat);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
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
    public void OnActionButtonDown() { actionPressed = true; }
    public void OnActionButtonUp() { actionPressed = false; }
}
