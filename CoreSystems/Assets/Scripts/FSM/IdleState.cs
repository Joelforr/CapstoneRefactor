using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public class IdleState : PlayerState {

    public IdleState(FSM parent)
    {
        this.sm = parent;
    }

    public override PlayerState HandleTransitions()
    {
        if(!sm._character.HasFlag(BaseCharacter.CollidedSurface.Ground)){
            return new FallState(sm);
        }
        else if (sm._character.directionalInput.x != 0 && sm._character.IsGrounded())
        {
            return new RunState(sm);
        }
        else if (sm._character.player.GetButtonDown(4))
        {
            return new JumpState(sm);
        }
        else if (sm._character.player.GetButtonDown(2) && sm.attackGraceFrames <= 0)
        {
            return new AttackState(sm);
        }
        else if (sm._character.player.GetButtonDown(3) && sm.dodgeGraceFrames <= 0)
        {
            return new DodgeState(sm);
        }
        else
        {
            return this;
        }
    }

    public override void OnStateEnter()
    {
        //Handler Setup
        eventManager.AddHandler<HitEvent>(OnHit);

        //Set Animation
        sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Idle) as XAnimation);

        sm.jumpCount = 0;
        //if (parent._velocity != Vector2.zero) parent._velocity = Vector2.zero;
        sm._character._velocity.y = 0;
    }

    public override void OnStateExit()
    {
        eventManager.RemoveHandler<HitEvent>(OnHit);
    }

    public override void Tick()
    {
        sm._character.SetFacing();
        sm._character._stamina.Regenerate();
        sm._character.staminaSystem.RegenTimerTick();
        sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, 0, sm._character.horizontal_drag /3);
        sm._character._velocity.y = 0;
    }
}
