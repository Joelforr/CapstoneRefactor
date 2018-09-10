using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRotation : MonoBehaviour {

    public GameObject orbPrefab;
    public int count;
    public int radius;
    public int test = 0;
    float lerpTime;
    Vector3 rotationalAnchor;

    private List<GameObject> list = new List<GameObject>();

	// Use this for initialization
	void Start () {
        DefineCircle();
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
        if (Input.GetKeyDown(KeyCode.R))
        {
            //StartCoroutine(Test());
            //Remove();

        }
	}

    void DefineCircle()
    {
        for(int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2 / count;
            Vector3 circlePos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            GameObject obj = Instantiate(orbPrefab, Vector3.zero + circlePos, Quaternion.identity);
            list.Add(obj);
        }
    }

    void Rotate()
    {
        for (int i = 0; i < count; i++){
            list[i].transform.RotateAround(Vector3.zero, Vector3.forward, 50f * Time.deltaTime);
        }
    }

    void Remove()
    {
        count--;
        list.Remove(list[0]);
        Resize();
    }

    void Resize()
    {
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2 / count;
            Vector3 circlePos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            list[i].transform.position = (Vector3.zero + circlePos);
        }
    }

    IEnumerator ShiftAnchors(Vector3 newAnchor)
    {
        Vector3 storedAnchor = rotationalAnchor;
        float currentLerpTime = 0;
        Vector3 offset = Vector3.zero;

        while (currentLerpTime < lerpTime)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;

            Vector3 value = Vector3.Lerp(storedAnchor, newAnchor, perc) - storedAnchor; 
        }
        yield return new WaitForSeconds(1);
            
    }
}
