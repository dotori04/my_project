using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public Transform suctionPoint;          // 흡입 위치 포인트
    public float suctionOffsetX = 1f;       // 좌우 반전 거리 오프셋
    public float suctionOffsetY = 0f;       // y 오프셋 (필요시 사용)

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1. 수평 입력 (a, d)
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            sr.flipX = moveInput < 0;
        }

        // 2. suctionPoint 위치 보정
        if (suctionPoint != null)
        {
            // x좌표: flipX 기준 좌우 오프셋
            Vector3 localPos = suctionPoint.localPosition;
            localPos.x = sr.flipX ? -suctionOffsetX : suctionOffsetX;
            suctionPoint.localPosition = localPos;

            // y좌표: Player의 현재 y좌표 따라감 (월드 기준)
            Vector3 suctionPos = suctionPoint.position;
            suctionPoint.position = new Vector3(
                suctionPos.x,
                transform.position.y + suctionOffsetY, // 필요시 y 오프셋 포함
                suctionPos.z
            );
        }

        // 3. 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 4. 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // 5. a/d 키에 따라 좌우 이동
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}
