using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Services {

    public static AnimationLibrary AnimationLibray;
    public static SFXLibrary SFXLibrary;

    public static void Init()
    {
        AnimationLibray = new AnimationLibrary();
        SFXLibrary = new SFXLibrary();
    }
}


