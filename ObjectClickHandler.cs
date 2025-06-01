using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ObjectClickHandler : MonoBehaviour
{
    public static ObjectClickHandler instance;

    public GameObject infoPopup;
    public TMP_Text nameText;
    public TMP_Text typeText;
    public TMP_Text descriptionText;
    public TMP_Text locationText;

    private List<ObjectData> objectInfoList;

    void Awake()
    {
        instance = this; // 전역 접근용 인스턴스 설정
    }

    void Start()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("object_data");
        ObjectData[] loadedData = JsonUtility.FromJson<Wrapper>("{\"objects\":" + jsonData.text + "}").objects;
        objectInfoList = new List<ObjectData>(loadedData);

        infoPopup.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))  // 오른쪽 클릭
        {
            if (infoPopup.activeSelf)
            {
                HidePopup();
            }
        }
    }

    public void ShowObjectInfo(string objectName)
    {
        foreach (var obj in objectInfoList)
        {
            if (obj.name == objectName)
            {
                nameText.text = "Name: " + obj.name;
                typeText.text = "Type: " + obj.type;
                descriptionText.text = "description: " + obj.description;
                locationText.text = "location: " + obj.location;

                infoPopup.SetActive(true);
                return;
            }
        }

        nameText.text = "None";
        typeText.text = "None";
        descriptionText.text = "None";
        locationText.text = "None";
        infoPopup.SetActive(true);
    }

    public void HidePopup()
    {
        infoPopup.SetActive(false);
    }

    [System.Serializable]
    private class Wrapper
    {
        public ObjectData[] objects;
    }
}
