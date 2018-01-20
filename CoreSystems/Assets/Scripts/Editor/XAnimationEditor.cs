using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class XAnimationEditor : EditorWindow {

    public XAnimation xAnimation;
    private int viewIndex = 1;

    private Rect leftPanel;
    private Rect rightPanel;
    private Rect resizer;

    private float sizeRatio;
    private bool isResizing;

    private GUIStyle resizerStyle;

    [MenuItem("Window/Custom/XAnimation Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(XAnimationEditor));
    }

    private void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            xAnimation = AssetDatabase.LoadAssetAtPath(objectPath, typeof(XAnimation)) as XAnimation;
        }

        sizeRatio = 0.3f;
        resizerStyle = new GUIStyle();
        resizerStyle.normal.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;
    }

    private void OnGUI()
    {
        DrawLeftPanel();
        DrawRightPanel();
        DrawResizer();

        ProcessEvents(Event.current);
        if (GUI.changed) Repaint();
    }

    private void DrawLeftPanel()
    {
        leftPanel = new Rect(0, 0, position.width * sizeRatio, position.height);
        GUILayout.BeginArea(leftPanel);
        GUILayout.Label("LeftPanel");
        GUILayout.EndArea();
    }

    private void DrawRightPanel()
    {
        rightPanel = new Rect((position.width * sizeRatio), 0, position.width * (1 - sizeRatio), position.height);
        GUILayout.BeginArea(rightPanel);
        GUILayout.Label("RightPanel");
        GUILayout.EndArea();
    }

    private void DrawResizer()
    {
        resizer = new Rect((position.width * sizeRatio) - 5f, 0,  10f, position.height);

        GUILayout.BeginArea(new Rect(resizer.position + (Vector2.right * 5f), new Vector2(1, position.height)), resizerStyle);
        GUILayout.EndArea();

        EditorGUIUtility.AddCursorRect(resizer, MouseCursor.ResizeHorizontal);
    }

    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if(e.button == 0 && resizer.Contains(e.mousePosition)){
                    isResizing = true;
                }
                break;

            case EventType.MouseUp:
                isResizing = false;
                break;
        }

        Resize(e);
    }

    private void Resize(Event e)
    {
        if (isResizing)
        {
            sizeRatio = e.mousePosition.x / position.width;
            sizeRatio = Mathf.Clamp(sizeRatio, .3f, .6f);
            Repaint();
        }
    }
}
