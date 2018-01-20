using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;

public class FallState : PlayerState
{

    public FallState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }

    public override PlayerState HandleTransitions()
    {
        if(Collisions.IsGrounded(parent.transform, parent._physicsCollider, parent._collisionMask))
        {
            return new IdleState(parent);
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
        parent.gravity = PhysX.CalculateGravity(parent.jump_height_max, parent.final_distance_to_peak, parent.horizontal_speed_max);
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        parent._velocity.y += parent.gravity * Time.deltaTime;

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

   
}
