using UnityEngine;

public class SuctionTrigger : MonoBehaviour
{
    public Transform suctionPoint;
    public LayerMask suckableLayer;
    public float suctionRange = 2f;

    private bool suctionEnabled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            suctionEnabled = !suctionEnabled;
            Debug.Log($"[SuctionTrigger] 흡입 모드: {(suctionEnabled ? "ON" : "OFF")}");
        }

        if (suctionEnabled)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, suctionRange, suckableLayer);

            foreach (var hit in hits)
            {
                var obj = hit.GetComponent<SuckableObject>();
                if (obj != null)
                {
                    if (obj.IsFired()) continue; // 발사 중인 오브젝트 흡입 불가

                    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector2.zero;
                        rb.angularVelocity = 0f;
                    }

                    obj.StartSuction(suctionPoint);
                }
            }
        }
    }
}
