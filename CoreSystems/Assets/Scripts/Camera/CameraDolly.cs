using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDolly : MonoBehaviour {

    [System.Serializable]
    public class LevelFocus
    {
        public Vector3 position;
        public  Vector3 halfBounds;

        public Bounds FocusBounds()
        {
            Bounds bounds = new Bounds();
            bounds.Encapsulate(position - halfBounds);
            bounds.Encapsulate(position + halfBounds);

            return bounds;
        }
    }


    public LevelFocus levelFocus;

    public List<Transform> focusObjects;
    private List<Vector3> focusPoints = new List<Vector3>();

    public float depthUpdateSpeed;
    public float angleUpdateSpeed;
    public float positionUpdateSpeed;

    public float depthMax;
    public float depthMin;

    public float angleMax;
    public float angleMin;

    private float targetEulerX;
    private Vector3 targetPos;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void LateUpdate () {
        focusPoints.Clear();
        focusPoints.Add(levelFocus.position);
        for (int i = 0; i < focusObjects.Count; i++)
        {
            focusPoints.Add(focusObjects[i].position);
        }

        CalculateCameraLocations();
        MoveCamera();
	}

    private void MoveCamera()
    {
        Vector3 position = Camera.main.transform.position;
        if(position != targetPos)
        {
            Camera.main.transform.position = Vector3.MoveTowards(position, targetPos, positionUpdateSpeed * Time.deltaTime);
        }

        Vector3 eulerAngles = Camera.main.transform.localEulerAngles;
        if(eulerAngles.x != targetEulerX)
        {
            Vector3 targetEuler = new Vector3(targetEulerX, eulerAngles.y, eulerAngles.z);
            Camera.main.transform.localEulerAngles = Vector3.MoveTowards(eulerAngles, targetEuler, angleUpdateSpeed * Time.deltaTime);
        }
    }

    private void CalculateCameraLocations()
    {

        Vector3 center = Vector3.zero;                                  //The center of all values in focusPoints
        Vector3 sum = Vector3.zero;                                     //The sum of all values in focusPoints
        Bounds playerBounds = new Bounds();

        for (int i = 0; i < focusPoints.Count; i++)
        {

            Vector3 pos = focusPoints[i];  
            if (!levelFocus.FocusBounds().Contains(focusPoints[i]))
            {
                pos.x = Mathf.Clamp(pos.x, levelFocus.FocusBounds().min.x, levelFocus.FocusBounds().max.x);
                pos.y = Mathf.Clamp(pos.y, levelFocus.FocusBounds().min.y, levelFocus.FocusBounds().max.y);
                pos.z = Mathf.Clamp(pos.z, levelFocus.FocusBounds().min.z, levelFocus.FocusBounds().max.z);
            }

            sum += pos;
            playerBounds.Encapsulate(pos);
        }

        center = (sum / focusPoints.Count);

        float extents = (playerBounds.extents.x + playerBounds.extents.y);
        float lerpPercent = Mathf.InverseLerp(0, (levelFocus.halfBounds.x + levelFocus.halfBounds.y) / 2, extents);

        float depth = Mathf.Lerp(depthMax, depthMin, lerpPercent);
        float angle = Mathf.Lerp(angleMax, angleMin, lerpPercent);

        targetEulerX = angle;
        targetPos = new Vector3(center.x, center.y, depth);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(levelFocus.position, .5f);
    }
}
