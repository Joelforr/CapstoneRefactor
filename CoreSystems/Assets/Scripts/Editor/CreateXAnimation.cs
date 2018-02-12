using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateXAnimation {
    private static int count = 0;

    [MenuItem("Assets/Create/Custom/XAnimation")]
    public static XAnimation Create()
    {
        XAnimation asset = ScriptableObject.CreateInstance<XAnimation>();

        if (!AssetDatabase.IsValidFolder("Assets/Data"))
            AssetDatabase.CreateFolder("Assets", "Data");

        if (!AssetDatabase.IsValidFolder("Assets/Data/XAnimationData")){
            AssetDatabase.CreateFolder("Assets/Data", "XAnimationData");
        }

        AssetDatabase.CreateAsset(asset, "Assets/Data/XAnimationData/XAnimation" + count + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        count++;
        return asset;
    }

}
