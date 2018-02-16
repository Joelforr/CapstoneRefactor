using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XFrame : ScriptableObject
{
    public Sprite sprite;
    public int lifetime;

    public List<HitboxProperties> hitboxes;
    public List<HitboxProperties> hurtboxes;
}

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
    
}

public class LayerProperties
{

}

[System.Serializable]
public class HitboxProperties
{
    public enum BoxType
    {
        Hit,
        Hurt,
        Physx
    }

    public BoxType type;
    public Rect _dimensions;
    public bool isTrigger = true;
    public float attack_damage = 0;
    public float knockback_growth = 0;
    public float base_knockback = 0;
    public float launch_angle = 0;
    
}
