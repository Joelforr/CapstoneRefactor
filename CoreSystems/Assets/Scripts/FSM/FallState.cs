using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public class FallState : PlayerState
{

    public FallState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }


    public override PlayerState HandleTransitions()
    {
        if(parent.HasFlag(Player.CollidedSurface.Ground))
        {
            return new IdleState(parent);
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
                return new AttackState(parent, AttackState.AttackType.Air);
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

        parent._xAnimator.SetAnimation(Resources.Load("Data/XAnimationData/Jump_XAnimation") as XAnimation);
        parent.gravity = PhysX.CalculateGravity(parent.jump_height_max, parent.final_distance_to_peak, parent.horizontal_air_speed);
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        parent._velocity.y += parent.gravity * Time.deltaTime;

        if (parent.IsPressingIntoLeftWall() || parent.IsPressingIntoRighttWall())
        {
            parent._velocity.x = 0f;
        }
        else
        {
            if (Mathf.Abs(parent.normalized_directional_input.x) > .2f)
            {
                parent._velocity.x += parent.normalized_directional_input.x * parent.horizontal_air_acceleration;
                //parent._velocity.x = Mathf.Max(Mathf.Min(parent._velocity.x, parent.horizontal_speed_max), -parent.horizontal_speed_max);
                if (parent._velocity.x > parent.horizontal_air_speed || parent._velocity.x < -parent.horizontal_air_speed)
                {
                    parent._velocity.x = parent._velocity.normalized.x * Mathf.MoveTowards(parent._velocity.magnitude, 0, parent.air_drag);
                }
            }
            else
            {
                parent._velocity.x = parent._velocity.normalized.x * Mathf.MoveTowards(parent._velocity.magnitude, 0, parent.air_drag);
            }
        }
    }

   
}
