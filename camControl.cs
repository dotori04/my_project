using UnityEngine;
using System.Collections;

public class camControl : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 20f;
    public float cameraSpeed = 3f;

    private Camera cam;
    public LayerMask starLayer;

    private Vector3 targetPosition;
    private bool isMovingToStar = false;

    private Vector3 lastMousePosition;
    private bool isDragging = false;
    private float clickThreshold = 10f; // 드래그와 클릭 구분 임계값
    private Coroutine currentMoveCoroutine = null;

    void Start()
    {
        cam = Camera.main;
        targetPosition = cam.transform.position;
    }

    void Update()
    {
        HandleMouseDragAndClick();
        HandleZoom();
        if (isMovingToStar)
        {
            if (Vector3.Distance(cam.transform.position, targetPosition) < 0.01f)
            {
                cam.transform.position = targetPosition;
                isMovingToStar = false;
            }
        }
    }

    void HandleMouseDragAndClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;

            // ✅ 드래그 시작 시 자동 이동 중단
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
                currentMoveCoroutine = null;
                isMovingToStar = false;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            if ((Input.mousePosition - lastMousePosition).magnitude < clickThreshold)
            {
                TryClickStar(); // 클릭 처리
            }
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = -delta * cam.orthographicSize / Screen.height * 2f;
            cam.transform.position += new Vector3(move.x, move.y, 0f);
            lastMousePosition = Input.mousePosition;
        }
    }

    void TryClickStar()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, starLayer);

        if (hit.collider != null)
        {
            Vector3 starPos = hit.collider.transform.position;
            Vector3 moveToPos = new Vector3(starPos.x, starPos.y, cam.transform.position.z);

            // ✅ 기존 코루틴이 있다면 중단
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }

            // ✅ 새 코루틴 시작 및 추적
            currentMoveCoroutine = StartCoroutine(MoveCameraTo(moveToPos));

            // 팝업 호출
            ObjectClickHandler.instance?.ShowObjectInfo(hit.collider.gameObject.name);
        }
    }


    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    public float GetZoomLev()
    {
        return cam.orthographicSize;
    }

    IEnumerator MoveCameraTo(Vector3 targetPos)
    {
        isMovingToStar = true;

        while (Vector3.Distance(cam.transform.position, targetPos) > 0.01f)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * cameraSpeed);
            yield return null;
        }

        cam.transform.position = targetPos;
        isMovingToStar = false;

        // 코루틴 종료 처리
        currentMoveCoroutine = null;
    }


}
