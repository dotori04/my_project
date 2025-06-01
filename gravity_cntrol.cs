using UnityEngine;

public class gravity_cntrol : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        switch (gameObject.name)
        {
            case "HeavyStar":
                rb.gravityScale = 3f;
                break;
            case "LightStar":
                rb.gravityScale = 0.5f;
                break;
            case "Star1":
                rb.gravityScale = 0f;
                break;
            default:
                rb.gravityScale = 1f; // 기본 중력
                break;
        }
    }
}
