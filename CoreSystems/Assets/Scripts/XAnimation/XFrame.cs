using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XFrame : ScriptableObject
{

    public Sprite sprite;
    public int frameLength;
    public List<Group> groups;
}

[System.Serializable]
public class Group
{
    public enum GroupName
    {
        Hit,
        Hurt,
        Physics
    }

    public GroupName name;

    //private string name;
    public List<HitboxProperties> boxes;
}

public class LayerProperties
{

}

[System.Serializable]
public class HitboxProperties
{
    public Vector4 rect;
}
