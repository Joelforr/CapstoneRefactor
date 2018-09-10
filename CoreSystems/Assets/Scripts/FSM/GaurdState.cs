using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public class GaurdState : PlayerState
{
    public GaurdState(FSM parent)
    {
        this.sm = parent;
    }

    public override PlayerState HandleTransitions()
    {
        return this;
    }

    protected override void OnAnimationComplete(AnimationCompleteEvent e)
    {
        sm.TransitionTo(new IdleState(sm));
    }

    public override void OnStateEnter()
    {
        eventManager.AddHandler<AnimationCompleteEvent>(OnAnimationComplete);
        sm._character._xAnimator.SetAnimation(Resources.Load("Data/XAnimationData/Gaurd_XAnimation") as XAnimation);
    }

    public override void OnStateExit()
    {
        
    }

    public override void Tick()
    {
        sm._character._velocity = Vector2.zero;
    }
}
