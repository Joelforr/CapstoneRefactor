using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAnimation : ScriptableObject {

    public int fps;
    public bool loop;
    public bool exitOnComplete;
    public List<XFrame> frames;


    public XFrame GetNextFrame(XFrame frame)
    {
        return frames.IndexOf(frame) + 1 > frames.Count-1 ?  frames[0] : frames[frames.IndexOf(frame) + 1];
    }

    public XFrame GetFinalXFrame()
    {
        return frames[frames.Count - 1];
    }

    public int GetTotalAnimationTime()
    {
        int num = 0;

        for (int i = 0; i < frames.Count; i++)
        {
            num += frames[i].lifetime;
        }

        return num;
    }

    
}

