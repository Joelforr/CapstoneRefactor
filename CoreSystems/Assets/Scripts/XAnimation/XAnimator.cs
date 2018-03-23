using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**********************************************************
 ***                                                    ***
 **********************************************************/

[RequireComponent(typeof(SpriteRenderer))]
public class XAnimator : MonoBehaviour {

    //Setup
    [Header("Setup")]
    public XAnimation default_xAnim;
    public int pool_size = 10;
    public GameObject CollisionBoxReference;

    [Header("Debug")]
    public bool DrawDebugBoxes = false;

    public int fps;
  
    
    //States
    [SerializeField]
    float currentFrameTime; //Refernce to the global time 
    [SerializeField]
    float nextFrameTime;

    [SerializeField]
    int keyframe_activeTime;
    int keyframe_count;
    int keyframe_startTime;

    private XAnimation active_xAnim;
    private XFrame active_keyframe;

    private float facing;

    //References 
    private SpriteRenderer frameRenderer;
    private Player parent_player;
    GameObject hurtbox_group = null;
    GameObject hitbox_group = null;

    //Storage
    List<GameObject> CollisionBoxPool = new List<GameObject>();
    List<GameObject> UsedCollisionBoxes = new List<GameObject>();



    /*********************************************Initial***************************************************/
    private void Awake()
    {
        InitializeCollisionBoxes();
    }

    private void Start()
    {
        frameRenderer = GetComponent<SpriteRenderer>();
        parent_player = GetComponent<Player>();
        Init();
        //foreach group in xAnim create a child game object with the name of that group
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        AdvanceFrames();
    }

    void InitializeCollisionBoxes()
    {
        for (int i = 0; i < pool_size; i++)
        {
            GameObject g = Instantiate(CollisionBoxReference, new Vector3(-10000, -10000, 0), Quaternion.identity, null) as GameObject;
            g.name = "UnusedCollisionBox";
            g.GetComponent<BoxCollider2D>().enabled = false;
            CollisionBoxPool.Add(g);
        }
    }

    /*******************************************************************************************************/


    /*********************************************Private***************************************************/
    /// <summary>
    /// Utility for advancing each frame when the last one has expired.
    /// Basically a tick counter
    /// </summary>
    void AdvanceFrames()
    {
        currentFrameTime = Time.timeSinceLevelLoad * 1000;

        if(currentFrameTime >= nextFrameTime)
        {
            keyframe_activeTime++;
            nextFrameTime = currentFrameTime + GetFrameTimeMilliseconds();
        }

        KeyframeTranstion();
        UpdateFacing();
        DrawCollisionBoxes();
        frameRenderer.sprite = active_keyframe.sprite;
    }

    private void DrawCollisionBoxes()
    {
        hurtbox_group = null;
        hitbox_group = null;

        foreach (Transform child in transform)
        {
            if (child.name == "Hurtbox")
            {
                hurtbox_group = child.gameObject;
            }
            if (child.name == "Hitbox")
            {
                hitbox_group = child.gameObject;
            }
        }

        if (hurtbox_group == null)
        {
            hurtbox_group = Xeo.Utility.CreateChildObj("Hurtbox", this.transform);
            hurtbox_group.AddComponent<CollisionGroupManager>();
            hurtbox_group.AddComponent<Rigidbody2D>().isKinematic = true;
        }
        if (hitbox_group == null)
        {
            hitbox_group = Xeo.Utility.CreateChildObj("Hitbox", this.transform);
            hitbox_group.AddComponent<CollisionGroupManager>();
            hitbox_group.AddComponent<Rigidbody2D>().isKinematic = true;
        }

        
        //Cleanup last frame's boxes
        if (UsedCollisionBoxes.Count > 0)
        {
            foreach (GameObject g in UsedCollisionBoxes)
            {
                g.GetComponent<CollisionBox>().Deactivate();
            }
        }
        UsedCollisionBoxes.Clear();

        //Add new boxes
        if (active_keyframe.hurtboxes.Count > 0)
        {
            hurtbox_group.GetComponent<CollisionGroupManager>().SetData(parent_player, active_keyframe.hurtboxes[0], facing);
            for (int i = 0; i < active_keyframe.hurtboxes.Count; i++)
            {
                AddCollisionBox("Hurtbox " + i, hurtbox_group.transform, active_keyframe.hurtboxes[i]);
            }
        }

        if (active_keyframe.hitboxes.Count > 0)
        {
            hitbox_group.GetComponent<CollisionGroupManager>().SetData(parent_player, active_keyframe.hitboxes[0], facing);
            for (int i = 0; i < active_keyframe.hitboxes.Count; i++)
            {
                AddCollisionBox("Hitbox " + i, hitbox_group.transform, active_keyframe.hitboxes[i]);
            }
        }

    }

    private void AddCollisionBox(string name, Transform parent, CollisionBoxData properties)
    {

        GameObject collision_box = null;
        foreach (GameObject g in CollisionBoxPool)
        {
            if (!g.GetComponent<CollisionBox>().isInUse)
            {
                collision_box = g;
                break;
            }
        }

        collision_box.GetComponent<CollisionBox>().Activate(name, parent, parent_player, properties, facing, DrawDebugBoxes);
        collision_box.GetComponent<CollisionBox>().DrawDebugBoxes();
        UsedCollisionBoxes.Add(collision_box);

        //GameObject collision_box = Xeo.Utility.CreateChildObj(name, parent);
        //collision_box.name = name;
        //collision_box.layer = LayerMask.NameToLayer(properties.type.ToString());
        //collision_box.transform.parent = parent;
        //collision_box.transform.localPosition = Vector3.zero;
        //collision_box.transform.position += new Vector3(
        //    properties._dimensions.position.x * facing,
        //    properties._dimensions.position.y);

        //BoxCollider2D _collider = collision_box.GetComponent<BoxCollider2D>();
        //_collider.isTrigger = properties.isTrigger;
        //_collider.size = properties._dimensions.size;


    }

    private void Init()
    {
        keyframe_activeTime = 0;
        keyframe_startTime = 0;

        try
        {
            active_keyframe = default_xAnim.frames[0];
        }
        catch
        {
            Debug.LogError("Missing default animation reference");
        }
    }

    /// <summary>
    /// Utility for transitioning keyframes once the currernt one has lived past it's lifetime.
    /// 
    /// </summary>
    void KeyframeTranstion()
    {
        if(keyframe_activeTime - keyframe_startTime > active_keyframe.lifetime)
        {
            keyframe_startTime = keyframe_activeTime;

            hitbox_group.GetComponent<CollisionGroupManager>().collided = false;
            hurtbox_group.GetComponent<CollisionGroupManager>().collided = false;

            if (active_keyframe == active_xAnim.GetFinalXFrame())
            {
                OnAnimationComplete();
            }
            else
            {
                active_keyframe = active_xAnim.GetNextFrame(active_keyframe);
            }
        }
    }

    /// <summary>
    /// Utility for handling animations once the last keyframe in the animation has been reached
    /// Loop or fire off completion event
    /// </summary>
    private void OnAnimationComplete()
    {
        switch (active_xAnim.loop)
        {
            case false:
                parent_player.mState.FireCustomEvent(new EventList.AnimationCompleteEvent(this));
                break;

            default:
                active_keyframe = active_xAnim.GetNextFrame(active_keyframe);
                break;
        }
        
    }

    private void UpdateFacing()
    {
        if (Mathf.Sign(frameRenderer.transform.localScale.x) != facing)
        {
            frameRenderer.transform.localScale = new Vector2(-frameRenderer.transform.localScale.x, frameRenderer.transform.localScale.y);
        }
    }

    /******************************************************************************************************/

    /*********************************************Public***************************************************/
    public float GetFrameTimeMilliseconds()
    {
        return ((1 / fps) * 1000);
    }

    public void SetAnimation(XAnimation animation)
    {
        keyframe_activeTime = 0;
        keyframe_startTime = 0;
        active_xAnim = animation;
        active_keyframe = active_xAnim.frames[0];
    }

    public void SetFacing(float direction_value)
    {
        facing = direction_value != 0 ?  direction_value : 1;
    }

    public int GetAnimationLengthFrames()
    {
        return active_xAnim.GetTotalFrameLength();
    }


   
}
