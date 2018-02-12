using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateXFrame  {
    private static int count = 0;

    [MenuItem("Assets/Create/Custom/XFrame")]
    public static XFrame Create()
    {
        XFrame asset = ScriptableObject.CreateInstance<XFrame>();

        if (!AssetDatabase.IsValidFolder("Assets/Data"))
            AssetDatabase.CreateFolder("Assets", "Data");

        if (!AssetDatabase.IsValidFolder("Assets/Data/XFrameData")) {
            AssetDatabase.CreateFolder("Assets/Data", "XFrameData");
        }
            

        AssetDatabase.CreateAsset(asset, "Assets/Data/XFrameData/XFrame" + count + ".asset");
        AssetDatabase.SaveAssets();
        count++;
        return asset;
    }
}
