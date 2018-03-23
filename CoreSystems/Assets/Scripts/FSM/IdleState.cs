﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public class IdleState : PlayerState {

    public IdleState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }

    public override PlayerState HandleTransitions()
    {
        if(!parent.HasFlag(Player.CollidedSurface.Ground)){
            return new FallState(parent);
        }
        else if (parent._inputManager.controller.x != 0 && parent.HasFlag(Player.CollidedSurface.Ground))
        {
            //OnStateExit();
            return new WalkState(parent);
        }
        else if (Input.GetButtonDown(parent._inputManager.jump))
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
        else if (Input.GetButtonDown(parent._inputManager.fire))
        {
            if (parent.stamina >= 10)
            {
                return new AttackState(parent, AttackState.AttackType.Ground);
            }
            else
            {
                return this;
            }
        }
        else
        {
            return this;
        }
    }

    public override void OnStateEnter()
    {
        eventManager.AddHandler<HitEvent>(OnHit);

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
        parent._velocity.x = parent._velocity.normalized.x * Mathf.MoveTowards(parent._velocity.magnitude, 0, parent.horizontal_drag/3);
        parent._velocity.y = 0;
    }
}
