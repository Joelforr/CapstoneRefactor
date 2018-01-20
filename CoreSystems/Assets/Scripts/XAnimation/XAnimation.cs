using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAnimation : ScriptableObject {

    public int fps;
    public bool loop;
    public bool exitOnComplete;
    public List<XFrame> frames;
}

