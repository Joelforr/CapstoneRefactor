using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAnimator : MonoBehaviour {

    public int fps;
    public XAnimation xAnim;
    
    [SerializeField]
    int current_frame;
    int total_frames;
    int last_keyframe;

    [SerializeField]
    float frame_current_time; //Refernce to the global time 
    [SerializeField]
    float frame_end_time;

    private XFrame active_frame;
    private SpriteRenderer frameRenderer;
    private Player parent_player;

    private float facing;


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
        AnimateFrames();
    }

    void AnimateFrames()
    {
        frame_current_time = Time.time * 1000;

        if(frame_current_time >= frame_end_time)
        {
            current_frame++;
            frame_end_time = frame_current_time + Mathf.CeilToInt((1 / fps) * 1000);
        }

        KeyframeTranstion();
        UpdateFacing();
        DrawCollisionBoxes();
        frameRenderer.sprite = active_frame.sprite;
    }

    private void Init()
    {
        current_frame = 0;
        last_keyframe = 0;

        try
        {
            active_frame = xAnim.frames[0];
        }
        catch
        {
            Debug.LogError("Missing animation reference");
        }
    }

    void KeyframeTranstion()
    {
        if(current_frame - last_keyframe > active_frame.lifetime)
        {
            last_keyframe = current_frame;

            if (active_frame == xAnim.GetFinalFrame())
            {
                OnAnimationComplete();
            }
            else
            {
                active_frame = xAnim.GetNextFrame(active_frame);
            }
        }
    }

    private void OnAnimationComplete()
    {
        switch (xAnim.loop)
        {
            case false:
                parent_player.XAnimatorCompletetionCall();
                break;

            default:
                active_frame = xAnim.GetNextFrame(active_frame);
                break;
        }
        
    }

    public void SetAnimation(XAnimation animation)
    {
        current_frame = 0;
        last_keyframe = 0;
        xAnim = animation;
        active_frame = xAnim.frames[0];
    }

    public void SetFacing(float direction_value)
    {
        facing = direction_value != 0 ?  direction_value : 1;
    }

    private void UpdateFacing()
    {
        if(Mathf.Sign(frameRenderer.transform.localScale.x) != facing)
        {
            frameRenderer.transform.localScale = new Vector2(-frameRenderer.transform.localScale.x, frameRenderer.transform.localScale.y);
        }
    }

    private void DrawCollisionBoxes()
    {
        GameObject hurtbox_group = null;
        GameObject hitbox_group = null;

        foreach (Transform child in transform)
        {
            if (child.name == "Hurtbox")
            {
                hurtbox_group = child.gameObject;
            }
            if(child.name == "Hitbox")
            {
                hitbox_group = child.gameObject;
            }
        }

        if (hurtbox_group == null)
        {
            hurtbox_group = Xeo.Utility.CreateChildObj("Hurtbox", this.transform);
        }
        if(hitbox_group == null)
        {
            hitbox_group = Xeo.Utility.CreateChildObj("Hitbox", this.transform);
        }

        ClearCollisionBoxes(hurtbox_group.transform);
        ClearCollisionBoxes(hitbox_group.transform);

        if(active_frame.hurtboxes.Count > 0)
        {
            for (int i = 0; i < active_frame.hurtboxes.Count; i++)
            {
                AddCollisionBox("Hurtbox " + i, hurtbox_group.transform, active_frame.hurtboxes[i]);
            }
        }

        if (active_frame.hitboxes.Count > 0)
        {
            for (int i = 0; i < active_frame.hitboxes.Count; i++)
            {
                AddCollisionBox("Hitbox " + i, hitbox_group.transform, active_frame.hitboxes[i]);
            }
        }

    }

    private void AddCollisionBox(string name, Transform parent, HitboxProperties properties)
    {
        GameObject collision_box = Xeo.Utility.CreateChildObj(name, parent);
        collision_box.layer = LayerMask.NameToLayer(properties.type.ToString());
        collision_box.transform.position += new Vector3(
            properties._dimensions.position.x * facing, 
            properties._dimensions.position.y);

        BoxCollider2D _collider = collision_box.AddComponent<BoxCollider2D>();
        _collider.isTrigger = properties.isTrigger;
        _collider.size = properties._dimensions.size;

        if (properties.type == HitboxProperties.BoxType.Hit)
        {
            Hitbox.AttachHitbox(collision_box, properties, facing);
        }
        
    }

    private void ClearCollisionBoxes(Transform box_group)
    {
        foreach(Transform hitbox in box_group)
        {
            Destroy(hitbox.gameObject);
        }
    }

}
