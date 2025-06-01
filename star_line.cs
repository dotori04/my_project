using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class star_line : MonoBehaviour
{
    public LayerMask starLayer;
    public float lineDrawSpeed = 2f;

    public static string selectedStarName; // ✅ 오타 수정
    private List<Transform> selectedStars = new List<Transform>();
    private LineRenderer line;
    private bool isDrawing = false;

    public List<Transform> specialSequence;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.widthMultiplier = 0.1f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.cyan;
        line.endColor = Color.cyan;
    }

    void Update()
    {
        // ⭐ 별 클릭 처리
        if (Input.GetMouseButtonDown(0) && !isDrawing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, starLayer);

            if (hit.collider != null)
            {
                Transform star = hit.collider.transform;

                // ✅ 클릭된 별 이름 저장
                selectedStarName = star.gameObject.name;
                Debug.Log($"별 선택됨: {selectedStarName}");

                if (!selectedStars.Contains(star))
                {
                    selectedStars.Add(star);

                    if (selectedStars.Count >= 2)
                    {
                        StartCoroutine(DrawAnimatedLine(
                            selectedStars[selectedStars.Count - 2].position,
                            selectedStars[selectedStars.Count - 1].position
                        ));
                    }
                    else
                    {
                        line.positionCount = 1;
                        line.SetPosition(0, star.position);
                    }

                    if (IsSpecialSequenceMatched())
                    {
                        TriggerSpecialEvent();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && !isDrawing)
        {
            selectedStars.Clear();
            line.positionCount = 0;
        }
    }

    IEnumerator DrawAnimatedLine(Vector3 start, Vector3 end)
    {
        isDrawing = true;

        int currentIndex = line.positionCount;
        line.positionCount += 1;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * lineDrawSpeed;
            Vector3 point = Vector3.Lerp(start, end, t);
            point.z = -0.1f;
            line.SetPosition(currentIndex, point);
            yield return null;
        }

        end.z = -0.1f;
        line.SetPosition(currentIndex, end);
        isDrawing = false;
    }

    bool IsSpecialSequenceMatched()
    {
        if (selectedStars.Count != specialSequence.Count)
            return false;

        for (int i = 0; i < specialSequence.Count; i++)
        {
            if (selectedStars[i] != specialSequence[i])
                return false;
        }

        return true;
    }

    void TriggerSpecialEvent()
    {
        Debug.Log("✨ 고래의 비밀이 밝혀졌습니다! 진엔딩 조건 달성!");
        // 진엔딩 관련 처리 추가 가능
    }
}
