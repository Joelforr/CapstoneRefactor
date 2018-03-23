using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public class WalkState : PlayerState {

    public WalkState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }

    public override PlayerState HandleTransitions()
    {
        if (!parent.HasFlag(Player.CollidedSurface.Ground))
        {
            return new FallState(parent);
        }
        else if (parent._velocity.x == 0 && Mathf.Abs(parent.normalized_directional_input.x) < 0.1f)
        {
            OnStateExit();
            return new IdleState(parent);
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
        else
        {
            return this;
        }
    }

    public override void OnStateEnter()
    {
        eventManager.AddHandler<HitEvent>(OnHit);

        parent._velocity.x += parent.normalized_directional_input.x * parent.horizontal_acceleration;
        parent._xAnimator.SetAnimation(Resources.Load("Data/XAnimationData/Run_XAnimation") as XAnimation);
    }

    public override void OnStateExit()
    {

    }

    public override void Tick()
    {
        parent.RegenStamina(.05f);
        //parent.stamina -= .45f;

        if (parent.IsPressingIntoLeftWall() || parent.IsPressingIntoRighttWall())
        {
            parent._velocity.x = 0f;
        }
        else
        {
            if (Mathf.Abs(parent.normalized_directional_input.x) > .5f)       //.5f deadzone
            {
                parent._velocity.x += parent.normalized_directional_input.x * parent.horizontal_acceleration;
                parent._velocity.x = Mathf.Max(Mathf.Min(parent._velocity.x, parent.horizontal_speed_max), -parent.horizontal_speed_max);
            }
            else
            {
                parent._velocity.x = parent._velocity.normalized.x * Mathf.MoveTowards(parent._velocity.magnitude, 0, parent.horizontal_drag);
            }
        }
    }
}
