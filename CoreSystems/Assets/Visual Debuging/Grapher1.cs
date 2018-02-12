using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapher1 : MonoBehaviour {
    [Range(0,200)]
    public int resolution;
    private int currentResolution;

    [Range(0,50)]
    public float axisLength;
    private float currentAxisLength;

    public ParticleSystem particleSystem;
    private ParticleSystem.Particle[] points;

    public Vector2 initPos;

    [Range(0,1)]
    public float precentage;
    public float baseDamage;
    public float knockbackConstant;
    public float knockbackGrowth;
    public float angle;
    
    public Vector2 vl;
    public float launchVelocity;
    private float currentLaunchVel;
    public float vlx;
    public float vly;
    public float gravity;
    public float hitstun;

    void Start () {
        CreatePoints();
        DrawGraph();
    }

    private void CreatePoints()
    {
        points = new ParticleSystem.Particle[resolution];
        float increment = axisLength / (resolution - 1);
        currentResolution = resolution;
        currentAxisLength = axisLength;
        currentLaunchVel = launchVelocity;
        for (int i = 0; i < resolution; i++)
        {
            float x = i * increment;
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].startColor = new Color(1, 0f, 0f);
            points[i].startSize = 0.2f;
        }
    }
	
	// Update is called once per frame
	void Update () {
     
        

        if (currentResolution != resolution || currentAxisLength != axisLength || currentLaunchVel != launchVelocity)
        {
            CreatePoints();
            DrawGraph();

        }

        


    }

    private void DrawGraph()
    {
        //launchVelocity = knockbackGrowth / 100f * (((14 * (Mathf.Lerp(1.4f, 1.0f, precentage) + baseDamage) * (baseDamage + 2)) / 140) + 18) + knockbackConstant;
        vl.x = Mathf.Cos(Mathf.Deg2Rad * angle) * launchVelocity;
        vl.y = Mathf.Sin(Mathf.Deg2Rad * angle) * launchVelocity;

        points[0].position = Vector3.zero;

        float testy = vl.y;

        for(int i = 1; i < resolution; i++)
        {
            points[i].position = points[i - 1].position + new Vector3(vl.x * Time.deltaTime, testy * Time.deltaTime);
            testy -= (gravity * Time.deltaTime);
        }

        particleSystem.SetParticles(points, points.Length);
    }
}
