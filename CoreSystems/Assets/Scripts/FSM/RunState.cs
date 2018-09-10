using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;


public class RunState : PlayerState {

    public RunState(FSM parent)
    {
        this.sm = parent;
    }

    public override PlayerState HandleTransitions()
    {
        if (!sm._character.HasFlag(BaseCharacter.CollidedSurface.Ground))
        {
            return new FallState(sm);
        }
        else if (sm._character._velocity.x == 0 && Mathf.Abs(sm._character.directionalInput.x) == 0f)
        {
            return new IdleState(sm);
        }
        else if (sm._character.player.GetButtonDown(2) && sm.attackGraceFrames <= 0)
        {
            return new AttackState(sm);
        }
        else if (sm._character.player.GetButtonDown(3) && sm.dodgeGraceFrames <= 0)
        {
            return new DodgeState(sm);
        }

        else if (sm._character.player.GetButtonDown(4))
        {
            return new JumpState(sm);
        }
        else
        {
            return this;
        }
    }

    public override void OnStateEnter()
    {
        eventManager.AddHandler<HitEvent>(OnHit);

        sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Run) as XAnimation as XAnimation);

        sm.jumpCount = 0;

        sm._character._velocity.x += sm._character.directionalInput.x * sm._character.horizontal_acceleration;
    }

    public override void OnStateExit()
    {
        eventManager.RemoveHandler<HitEvent>(OnHit);

    }

    public override void Tick()
    {
        sm._character.SetFacing();

        sm._character._stamina.Regenerate(.5f);
        sm._character.staminaSystem.RegenTimerTick();



        if (sm._character.IsPressingIntoLeftWall() || sm._character.IsPressingIntoRighttWall())
        {
            sm._character._velocity.x = 0f;
        }
        else
        {
            if (sm._character.directionalInput.x != 0)
            {
                sm._character._velocity.x += sm._character.directionalInput.x * sm._character.horizontal_acceleration;
                sm._character._velocity.x = Mathf.Max(Mathf.Min(sm._character._velocity.x, sm._character.horizontal_speed_max), -sm._character.horizontal_speed_max);
            }
            else
            {


                sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, 0, sm._character.horizontal_drag);
            }
        }
    }
}
