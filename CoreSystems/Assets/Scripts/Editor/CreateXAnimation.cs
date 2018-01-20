using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateXAnimation {
    [MenuItem("Assets/Create/Custom/XAnimation")]
    public static XAnimation Create()
    {
        XAnimation asset = ScriptableObject.CreateInstance<XAnimation>();

        if (!AssetDatabase.IsValidFolder("Assets/Data"))
            AssetDatabase.CreateFolder("Assets", "Data");

        if (!AssetDatabase.IsValidFolder("Assets/Data/XAnimationData"))
            AssetDatabase.CreateFolder("Asstes/Data", "XAnimationData");

        AssetDatabase.CreateAsset(asset, "Assets/Data/XAnimationData/XAnimation.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
