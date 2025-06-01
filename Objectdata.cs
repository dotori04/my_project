[System.Serializable]
public class ObjectData
{
    public string name;
    public string type;
    public string description;
    public string location;
}

[System.Serializable]
public class ObjectDataList
{
    public ObjectData[] objects;
}
