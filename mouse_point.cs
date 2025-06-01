using UnityEngine;
using System.Collections;

public class mouse_point : MonoBehaviour
{
    public float followSpeed = 10f;
    public Color clickedColor = new Color(171f,171f,171f);
    public float colorChangeDuration = 1f;
    public float mouseSize = 1f;
    public float camSzie;
    // public float b = mouseSize * camSzie;
    private SpriteRenderer sr;
    private Color OwnColor;
    public camControl cam;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // ColorUtility.TryParseHtmlString("ABABAB",out clickedColor);
        OwnColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        camSzie = cam.GetZoomLev();
        FollowMouse();

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            StartCoroutine(ChangeColorTemporarily());
        }
        // b = mouseSize * camSzie;
        transform.localScale = new Vector3(mouseSize * camSzie,mouseSize * camSzie,0f);

    }

    void FollowMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = Vector3.Lerp(transform.position, mouseWorldPos, followSpeed*Time.deltaTime);
    }

    IEnumerator ChangeColorTemporarily()
    {
        sr.color = clickedColor;
        yield return new WaitForSeconds(colorChangeDuration);
        sr.color = OwnColor;
    }
}
