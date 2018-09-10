using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina {

    public float base_value;


    float current;
    public float Current
    {
        get { return current; }
        set { current = value;
            current = Mathf.Clamp(current, 1, current_max); }
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


    public float regen_rate;

    public Stamina(float base_v, float regen_v)
    {
        base_value = base_v;
        current_max = base_v;
        current = base_v;
        regen_rate = regen_v;
    }


    public float GetPercentage()
    {
        current = Mathf.Clamp(current, 0, current_max);
        return current / current_max;
    }

    public float GetBasePercentageChange()
    {
        return (Max - base_value) / base_value;
    }

    public void Regenerate(float multiplier = 1.0f)
    {
        Current += (regen_rate * multiplier);
    }

    public void Reset()
    {
        Max = base_value;
        Current = Max;
    }

    public void Siphon(Stamina other, float value)
    {
        other.Current -= value;

        if (other.Max >= value / 2)
        {
            Max += value / 2;
            other.Max -= value / 2;
        }
    }

    public void Siphon(BaseCharacter other, float value)
    {
        other._stamina.Current -= value;
        if (other._stamina.Max >= value / 2)
        {
            Max += value / 2;
            other._stamina.Max -= value / 2;
        }
    }
  
}
