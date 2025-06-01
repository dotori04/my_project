using UnityEngine;

public class SuckableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform suctionPoint;

    private bool isBeingPulled = false;
    private bool isLocked = false;
    private bool isFired = false;

    public float suctionSpeed = 5f;
    public float fireForce = 10f;
    public Vector3 suctionOffset;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
    }

    void Update()
    {
        if (isBeingPulled && suctionPoint != null && !isLocked)
        {
            Vector3 target = suctionPoint.position + suctionOffset;
            transform.position = Vector3.MoveTowards(transform.position, target, suctionSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                isBeingPulled = false;
                isLocked = true;

                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        if (isLocked && suctionPoint != null)
        {
            transform.position = suctionPoint.position + suctionOffset;

            if (Input.GetMouseButtonDown(0))
            {
                FireTowardsMouse();
            }
        }
    }

    void FireTowardsMouse()
    {
        if (isFired) return; // 중복발사 방지

        isLocked = false;
        isFired = true;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector2 direction = (mouseWorldPos - transform.position).normalized;

        rb.AddForce(direction * fireForce, ForceMode2D.Impulse);
        Debug.Log($"{gameObject.name} → 발사됨");
    }

    public void StartSuction(Transform point)
    {
        // 흡입 조건: 이미 흡입 중이거나, 고정 중이거나, 발사 중이면 흡입 불가
        if (isBeingPulled || isLocked || isFired) return;

        suctionPoint = point;
        isBeingPulled = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            if (isFired)
            {
                Debug.Log($"{gameObject.name} → 땅에 착지, 즉시 파괴됨");
                Destroy(gameObject);
            }
        }
    }

    public bool IsFired()
    {
        return isFired;
    }
}
