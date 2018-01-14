using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerState {

    public WalkState(Player parent)
    {
        this.parent = parent;
    }


    public override PlayerState HandleTransitions()
    {
        if (parent._velocity.x == 0)
        {
            return new IdleState(parent);
        }
        else
        {
            return this;
        }
    }

    public override void Tick()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > .2f)
        {
            parent._velocity.x += Input.GetAxis("Horizontal") * parent.horizontal_acceleration;
            parent._velocity.x = Mathf.Max(Mathf.Min(parent._velocity.x, parent.horizontal_speed_max), -parent.horizontal_speed_max);
        }
        else
        {
            parent._velocity.x = parent._velocity.normalized.x * Mathf.MoveTowards(parent._velocity.magnitude, 0, parent.horizontal_drag);
        }
       

    }

    public override void OnStateEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

}
