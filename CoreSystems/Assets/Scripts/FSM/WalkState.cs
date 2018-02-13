using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerState {

    public WalkState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }

    public override void AnimationTransitionEvent()
    {

    }

    public override PlayerState HandleTransitions()
    {
        if (parent._velocity.x == 0 && Mathf.Abs(parent.normalized_directional_input.x) < 0.1f)
        {
            OnStateExit();
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
        parent._velocity.x += parent.normalized_directional_input.x * parent.horizontal_acceleration;
        parent._xAnimator.SetAnimation(Resources.Load("Data/XAnimationData/Run_XAnimation") as XAnimation);
    }

    public override void OnStateExit()
    {

    }

    public override void Tick()
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
