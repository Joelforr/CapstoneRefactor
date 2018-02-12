using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grapher2 : MonoBehaviour {

    public Material mat;

    public int frameDuration;
    public Vector2 initPos;

    [Range(0, 1)]
    public float precentage;
    public float baseDamage;
    public float knockbackConstant;
    public float knockbackGrowth;
    public float angle;

    public Vector2 vl;
    public float launchVelocity;
    private float currentLaunchVel;

    public float gravity;
    public float hitstun;

    private Vector3[] points;

    private void Awake()
    {
        CreatePoints();
    }

    // Use this for initialization
    void Start () {
		
	}

    void CreatePoints()
    {
        points = new Vector3[frameDuration];
    }
	
	void OnPostRender()
    {
        
        launchVelocity = knockbackGrowth / 100f * (((14 * (Mathf.Lerp(1.4f, 1.0f, precentage) + baseDamage) * (baseDamage + 2)) / 140) + 18) + knockbackConstant;
        vl.x = Mathf.Cos(Mathf.Deg2Rad * angle) * launchVelocity;
        vl.y = Mathf.Sin(Mathf.Deg2Rad * angle) * launchVelocity;

        points[0] = initPos;
        Vector3 value = vl;

        GL.PushMatrix();
        mat.SetPass(0);
        GL.Begin(GL.LINE_STRIP);
        GL.Color(new Color(0, 0, 0, 1f));
        GL.Vertex3(0f,0f,0f);
        for (int i = 1; i < points.Length; i++)
        {
            points[i] = (points[i-1] + new Vector3(value.x * Time.fixedDeltaTime, value.y * Time.fixedDeltaTime));
            value.y -= gravity * Time.fixedDeltaTime;
            GL.Vertex(points[i]);
        }
        GL.End();
        GL.PopMatrix();
    }

}
