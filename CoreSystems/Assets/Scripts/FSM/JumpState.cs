using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;

public class JumpState: PlayerState {

    public JumpState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }

    public override PlayerState HandleTransitions()
    {
        if(parent._velocity.y < 0)
        {
            return new FallState(parent);
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
        parent.gravity = PhysX.CalculateGravity(parent.jump_height_max, parent.initial_distance_to_peak, parent.horizontal_speed_max);
        parent._velocity.y = PhysX.CalculateJumpVelocity(parent.jump_height_max, parent.initial_distance_to_peak, parent.horizontal_speed_max);
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
