using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameAnimateTest : MonoBehaviour {

    /*
    public int animationFramesPerSecond = 15;
    public Animations[] animationList;

    public Color hitBoxColor = Color.green; // customize in inspector with lower alpha, etc.
    public Color hurtBoxColor = Color.red;
    public int pixelsPerUnit; // use whatever your other sprites use
    SpriteRenderer hurtBox;
    SpriteRenderer hitBox;

    [HideInInspector]
    public Animations currentAnimation;
    int currentAnimationFrame = 0;
    float frameCurrenTime = 0;
    float frameEndTime = 0;
    SpriteRenderer frameRenderer;

    //useful for figuring out whether to do a check or not
    [HideInInspector]
    public bool hasHurtBoxThisFrame = true;
    [HideInInspector]
    public bool hasHitBoxThisFrame = true;

    [System.Serializable]
    public class Animations
    {
        public string animationName;
        public FrameInfo[] frames;
    }
    [System.Serializable]
    public class FrameInfo
    {
        public Sprite frameSprite;
        public HitBox[] HitBoxes;
        public HurtBox[] HurtBoxes;
    }
    [System.Serializable]
    public class HitBox
    {
        public Rect hitBoxDimensions;
    }
    [System.Serializable]
    public class HurtBox
    {
        public Rect hurtBoxDimensions;
    }

    private void Awake()
    {
        frameRenderer = GetComponent<SpriteRenderer>();

        hurtBox = new GameObject("hurtBox", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
        hurtBox.sprite = CreateBoxSprite();
        hurtBox.color = Color.clear;

        hitBox = new GameObject("hitBox", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
        hitBox.sprite = CreateBoxSprite();
        hitBox.color = Color.clear;
    }

    private Sprite CreateBoxSprite()
    {
        // create 1x1 white pixel
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();

        // create a new sprite with the texture applied
        Sprite sprite = new Sprite();
        sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), this.pixelsPerUnit);

        return sprite;
    }

    // Use this for initialization
    void Start()
    {
        SetAnimation("Stand");
    }

    // Update is called once per frame
    void Update()
    {
        AnimateFrames();
        DrawBoxes();
    }

    void AnimateFrames()
    {
        frameCurrenTime = Time.time * 1000;

        if (frameCurrenTime >= frameEndTime)
        {
            currentAnimationFrame++;
            frameEndTime = frameCurrenTime + Mathf.CeilToInt((1f / animationFramesPerSecond) * 1000);
        }

        if (currentAnimationFrame > currentAnimation.frames.Length - 1)
        {
            currentAnimationFrame = 0;
        }

        frameRenderer.sprite = currentAnimation.frames[currentAnimationFrame].frameSprite;
    }

    public bool SetAnimation(string animationName, int startFrame = 0)
    {
        //Is our current animation the same as the one we are setting?
        if (currentAnimation.animationName.ToLower() == animationName.ToLower())
            return true;

        for (int i = 0; i < animationList.Length; i++)
        {
            if (animationList[i].animationName.ToLower() == animationName.ToLower())
            {
                if (startFrame > animationList[i].frames.Length - 1)
                    startFrame = animationList[i].frames.Length - 1;
                if (startFrame < 0)
                {
                    startFrame = 0;
                }

                currentAnimationFrame = startFrame;
                currentAnimation = animationList[i];

                return true;
            }
        }

        Debug.Log("Error, animation not registered.");
        return false;
    }

    void DrawBoxes()
    {
        Vector2 mePosition = transform.position;
        FrameInfo currentFrame = currentAnimation.frames[currentAnimationFrame];
        HurtBox[] hurtBoxes = currentFrame.HurtBoxes;
        HitBox[] hitBoxes = currentFrame.HitBoxes;

        if (hurtBoxes.Length != 0)
        {
            //hurtboxes
            for (int i = 0; i < hurtBoxes.Length; i++)
            {
                Rect hurtBoxRect = new Rect(hurtBoxes[i].hurtBoxDimensions.position + mePosition, hurtBoxes[i].hurtBoxDimensions.size);

                this.hurtBox.transform.position = new Vector2(hurtBoxRect.x, hurtBoxRect.y);
                this.hurtBox.transform.localScale = new Vector2(hurtBoxRect.width, hurtBoxRect.height);
                this.hurtBox.color = this.hurtBoxColor;
            }
        }
        else
        {
            hasHurtBoxThisFrame = false;
            this.hurtBox.color = Color.clear;
        }

        if (hitBoxes.Length != 0)
        {
            //hitboxes
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                Rect hitBoxRect = new Rect(hitBoxes[i].hitBoxDimensions.position + mePosition, hitBoxes[i].hitBoxDimensions.size);

                this.hitBox.transform.position = new Vector2(hitBoxRect.x, hitBoxRect.y);
                this.hitBox.transform.localScale = new Vector2(hitBoxRect.width, hitBoxRect.height);
                this.hitBox.color = this.hitBoxColor;
            }
        }
        else
        {
            hasHitBoxThisFrame = false;
            this.hitBox.color = Color.clear;
        }
    }


    //end
    */
}
