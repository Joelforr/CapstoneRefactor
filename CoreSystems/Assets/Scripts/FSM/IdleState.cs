using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;

public class IdleState : PlayerState {

    public IdleState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }

    public override PlayerState HandleTransitions()
    {
        if(!Collisions.IsGrounded(parent.transform, parent._physicsCollider, parent._collisionMask)){
            return new FallState(parent);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            //OnStateExit();
            return new WalkState(parent);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return new JumpState(parent);
        }
        else
        {
            return this;
        }
    }

    public override void OnStateEnter()
    {
        //if (parent._velocity != Vector2.zero) parent._velocity = Vector2.zero;
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        parent._velocity = Vector2.zero;
    }
}
