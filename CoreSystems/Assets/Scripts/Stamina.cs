using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour {

    float base_value;

    float current;
    public float Current
    {
        get { return current; }
        set { current = value;
            current = Mathf.Clamp(current, 0, current_max); }
    }

    float current_max;
    public float Max
    {
        get { return current_max; }
        set { current_max = value;
            if (current_max <= 0)
            {
                //Stamina pool > 0
                //Handle Reset
            }
            else {
                current = Mathf.Clamp(current, 0, current_max);
            }
        }
    }


    float regen_rate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetPercentage()
    {
        current = Mathf.Clamp(current, 0, current_max);
        return current / current_max;
    }

    public void Siphon(Stamina other, float value)
    {
        Max += value;
        other.Max -= value;
    }

    public void Regenerate(float multiplier = 1.0f)
    {
        Current += (regen_rate * multiplier);
    }



  
}
