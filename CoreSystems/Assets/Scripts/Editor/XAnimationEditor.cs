using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Xeo;

public class XAnimationEditor : EditorWindow {

    public XAnimation xAnimation;

    private XAnimation xAnimation_obj;
    private List<XFrame> child_frames;
    private XFrame current_xframe;


    private int viewIndex = 1;

    private Rect leftPanel;
    private Rect rightPanel;
    private Rect resizer;

    private float sizeRatio;
    private bool isResizing;
    private GUIStyle resizerStyle;

    private GUIStyle foldoutStyle;
    private bool show_properties;
    private bool show_physbox;
    private bool show_hitbox;
    private bool show_hurtbox;


 [MenuItem("Window/Custom/XAnimation Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(XAnimationEditor));
    }

    public XAnimationEditor()
    {
        child_frames = new List<XFrame>();
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

        foldoutStyle = new GUIStyle();
        foldoutStyle.normal.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;

    }

    private void OnSelectionChange()
    {
        if(Selection.objects.Length > 1)
        {

        }
        else if(Selection.activeObject is XAnimation)
        {
            child_frames.Clear();
            xAnimation_obj = (XAnimation)Selection.activeObject;
            foreach(XFrame frame in xAnimation_obj.frames)
            {
                child_frames.Add(frame);
            }
        }
        else
        {
            child_frames.Clear();
        }

        this.Repaint();
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

        if (xAnimation_obj != null)
        {
            GUILayout.Label("LeftPanel", EditorStyles.boldLabel);
            show_properties = EditorGUILayout.Foldout(show_properties, "Properties");
            if (show_properties)
            {
                EditorGUILayout.TextField("Current Animation:", xAnimation_obj.name);
            }
            show_physbox = EditorGUILayout.Foldout(show_physbox, "Physics Collider");
        }
        GUILayout.EndArea();
    }

    private void DrawRightPanel()
    {
        rightPanel = new Rect((position.width * sizeRatio), 0, position.width * (1 - sizeRatio), position.height);
        GUILayout.BeginArea(rightPanel);
        GUILayout.Label("RightPanel");
        GUILayout.EndArea();
        DrawOnGUISprite(child_frames[0].sprite);
        //GUI.DrawTextureWithTexCoords(new Rect(0,0,64,64), Xeo.Utility.textureFromSprite(child_frames[0].sprite), Xeo.Utility.textureFromSprite(child_frames[0].sprite).r);
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

    void DrawOnGUISprite(Sprite aSprite)
    {
        Rect c = aSprite.rect;
        float spriteW = c.width;
        float spriteH = c.height;
        Rect rect = GUILayoutUtility.GetRect(spriteW, spriteH);
        if (Event.current.type == EventType.Repaint)
        {
            var tex = Xeo.Utility.textureFromSprite(aSprite);
            c.xMin /= tex.width;
            c.xMax /= tex.width;
            c.yMin /= tex.height;
            c.yMax /= tex.height;
            GUI.DrawTextureWithTexCoords(rect, tex, c);
        }
    }
}
