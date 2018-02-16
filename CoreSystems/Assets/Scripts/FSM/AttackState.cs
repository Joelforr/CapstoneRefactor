using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;

public class AttackState : PlayerState {

    public enum AttackType
    {
        Ground,
        Air
    }

    private AttackType attack_type;

    public AttackState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }

    public AttackState(Player parent, AttackType atk_type)
    {
        this.parent = parent;
        this.attack_type = atk_type;
        OnStateEnter();
    }

    public override void AnimationTransitionEvent()
    {
        parent.SetState(new IdleState(parent));
    }

    public override PlayerState HandleTransitions()
    {
        return this;
    }

    public override void OnStateEnter()
    {
        //Switch statement
        //Play attack animation based off atk_type & direction
        //Animation handles state switching
        parent._xAnimator.SetAnimation(Resources.Load("Data/XAnimationData/F_Slash_XAnimation") as XAnimation);

    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
       
    }

}
