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

    public override void AnimationTransitionEvent()
    {

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
            if (parent.stamina >= 15)
            {
                return new JumpState(parent);
            }
            else
            {
                return this;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            return new AttackState(parent);
        }
        else
        {
            return this;
        }
    }

    public override void OnStateEnter()
    {
        parent._xAnimator.SetAnimation(Resources.Load("Data/XAnimationData/Idle_XAnimation") as XAnimation);
        //if (parent._velocity != Vector2.zero) parent._velocity = Vector2.zero;
        
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        parent.RegenStamina();
        parent._velocity = Vector2.zero;
    }
}
