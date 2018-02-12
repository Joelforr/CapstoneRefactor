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
}

