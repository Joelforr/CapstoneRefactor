using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState {

    public IdleState(Player parent)
    {
        this.parent = parent;
    }

    public override PlayerState HandleTransitions()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            return new WalkState(parent);
        }
        else
        {
            return this;
        }
    }

    public override void Tick()
    {
        Debug.Log(this.GetType());
        parent._velocity = Vector2.zero;
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
