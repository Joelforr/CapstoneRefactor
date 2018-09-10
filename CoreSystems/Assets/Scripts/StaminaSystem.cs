using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour {

    public GameObject orbPrefab;
    public Vector3 offset;
    private Vector3 center;
    public float radius;
    public float rotationSpeed;

    public int baseStamina;

    private int timer = 40;
    private int currentTime = 0;

    private Stack<GameObject> activeOrbs = new Stack<GameObject>();
    private Stack<GameObject> deactiveOrbs = new Stack<GameObject>();
    private List<GameObject> exsistingOrbs = new List<GameObject>();

    private void Start()
    {
        center = transform.position + offset;
        for (int i = 0; i < baseStamina; i++)
        {
    
            float angle = i * Mathf.PI * 2 / baseStamina;
            Vector3 circlePos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            activeOrbs.Push(Instantiate(orbPrefab, center + circlePos, Quaternion.identity, this.transform));
        }

        UpdateExsistingOrbs();
    }

    private void Update()
    {
        center = transform.position + offset;

        for (int i = 0; i < exsistingOrbs.Count; i++)
        {
           exsistingOrbs[i].transform.RotateAround(center, Vector3.forward, rotationSpeed * Time.deltaTime);
        }


        if (deactiveOrbs.Count > 0)
        {

            if (currentTime > (timer / 4))
            {
                GameObject temp = deactiveOrbs.Peek();
                temp.GetComponent<SpriteRenderer>().enabled = true;
                float perc = (float)currentTime / (float)timer;
                perc = 1f - Mathf.Cos(perc * Mathf.PI * 0.5f);
                temp.gameObject.transform.localScale = Vector3.Lerp(Vector2.zero, Vector2.one, (perc));
            }

            if (currentTime >= timer)
            {
                Activate();
                currentTime = 0;
            }
        }
     

        #region debugging
        if (Input.GetKeyDown(KeyCode.A))
        {
            Activate();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Deactivate();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Add();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Remove();
        }
        #endregion
    }

    private void ComputeOrbit()
    {
        for(int i = 0; i < exsistingOrbs.Count; i++)
        {
            float angle = i * Mathf.PI * 2 / exsistingOrbs.Count;
            Vector3 circlePos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            exsistingOrbs[i].transform.position = center + circlePos;
        }
    }

    private void UpdateExsistingOrbs()
    {
        exsistingOrbs.Clear();

        foreach (GameObject orb in activeOrbs)
        {
            exsistingOrbs.Add(orb);
        }

        foreach (GameObject orb in deactiveOrbs)
        {
            exsistingOrbs.Add(orb);
        }

        exsistingOrbs.Reverse();
    
    }

    private void Activate()
    {
        GameObject temp = deactiveOrbs.Pop();
        activeOrbs.Push(temp);
        temp.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Add()
    {
        activeOrbs.Push(Instantiate(orbPrefab,this.transform));
        UpdateExsistingOrbs();
        ComputeOrbit();
    }

    private void Deactivate()
    {
        Debug.Assert(activeOrbs.Count > 0);
        GameObject temp = activeOrbs.Pop();
        deactiveOrbs.Push(temp);
        temp.GetComponent<SpriteRenderer>().enabled = false;
    }


    public int GetCurrentStamina()
    {
        return activeOrbs.Count;
    }

    public int GetCurrentMaxStamina()
    {
        return exsistingOrbs.Count;
    }

    public void RegenTimerTick()
    {
        if (deactiveOrbs.Count > 0)
        {
            this.currentTime++;
        }
    }

    public void Remove()
    {
        GameObject.Destroy(activeOrbs.Count > 0 ? activeOrbs.Pop() : deactiveOrbs.Pop());
        UpdateExsistingOrbs();
        ComputeOrbit();
    }

    public void Reset()
    {
        activeOrbs.Clear();
        deactiveOrbs.Clear();

        for(int i = 0; i < exsistingOrbs.Count; i++) {
            GameObject temp = exsistingOrbs[i];
            //exsistingOrbs.Remove(temp);
            GameObject.Destroy(temp);
        }

        for (int i = 0; i < baseStamina; i++)
        {
            float angle = i * Mathf.PI * 2 / baseStamina;
            Vector3 circlePos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            activeOrbs.Push(Instantiate(orbPrefab, center + circlePos, Quaternion.identity,this.transform));
        }

        UpdateExsistingOrbs();
    }


    public void SpendOrbs(int amount)
    {
        if (amount == 0) return;

        for (int i = 0; i < amount; i++)
        {
            if (deactiveOrbs.Count > 0)
            {
                GameObject temp = deactiveOrbs.Peek();
                temp.GetComponent<SpriteRenderer>().enabled = false;
            }

            Deactivate();
        }
    }

}
