using System;
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


    private void Start()
    {
        frameRenderer = GetComponent<SpriteRenderer>();
        Init();
        //foreach group in xAnim create a child game object with the name of that group
    }

    private void Update()
    {
        AnimateFrames();
    }

    private void FixedUpdate()
    {
        
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
        frameRenderer.sprite = active_frame.sprite;
    }
    
    void KeyframeTranstion()
    {
        if(current_frame - last_keyframe > active_frame.lifetime)
        {
            last_keyframe = current_frame;
            active_frame = xAnim.GetNextFrame(active_frame);
        }
    }

    public void SetAnimation(XAnimation animation)
    {
        current_frame = 0;
        last_keyframe = 0;
        xAnim = animation;
        active_frame = xAnim.frames[0];
    }

    private void Init()
    {
        current_frame = 0;
        last_keyframe = 0;

        try{
            active_frame = xAnim.frames[0];
        }
        catch (NullReferenceException e)
        {
            Debug.LogError("Missing animation reference");
        }
    }

}
