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

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data"))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Data");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/XFrameData")) {
            AssetDatabase.CreateFolder("Assets/Resources/Data", "XFrameData");
        }
            

        AssetDatabase.CreateAsset(asset, "Assets/Resources/Data/XFrameData/XFrame" + count + ".asset");
        AssetDatabase.SaveAssets();
        count++;
        return asset;
    }
}
