using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public class FallState : PlayerState
{

    public FallState(FSM parent)
    {
        this.sm = parent;
    }


    public override PlayerState HandleTransitions()
    {
        if(sm._character.HasFlag(BaseCharacter.CollidedSurface.Ground))
        {
            return new IdleState(sm);
        }
        else if (sm._character.player.GetButtonDown(4) && sm.jumpGraceFrames <= 0 && sm.jumpCount < sm.jumpsAllowed)
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
        sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Fall) as XAnimation);

        //Pre-Calculations
        sm._character.gravity = PhysX.CalculateGravity(sm._character.jump_height_max, sm._character.final_distance_to_peak, sm._character.horizontal_air_speed);
    }

    public override void OnStateExit()
    {
        eventManager.RemoveHandler<HitEvent>(OnHit);
    }

    public override void Tick()
    {
        sm._character.SetFacing();

        if (sm._character._velocity.y > sm._character.gravity_cutoff)
        {
            sm._character._velocity.y += sm._character.gravity * Time.deltaTime;
        }
        else if(sm._character._velocity.y < sm._character.gravity_cutoff)
        {

        }

        if (sm._character.IsPressingIntoLeftWall() || sm._character.IsPressingIntoRighttWall())
        {
            sm._character._velocity.x = 0f;
        }
        else
        {
            if (Mathf.Abs(sm._character.directionalInput.x) > .2f)
            {
                sm._character._velocity.x += sm._character.directionalInput.x * sm._character.horizontal_air_acceleration;
                //parent._velocity.x = Mathf.Max(Mathf.Min(parent._velocity.x, parent.horizontal_speed_max), -parent.horizontal_speed_max);

                if (sm._character._velocity.x > sm._character.horizontal_speed_max)
                {
                    sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, sm._character.horizontal_speed_max, sm._character.air_drag);
                }

                if (sm._character._velocity.x < -sm._character.horizontal_speed_max)
                {
                    sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, -sm._character.horizontal_speed_max, sm._character.air_drag);
                }

            }
            else
            {
                sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, 0, sm._character.air_drag);
            }
        }
    }

   
}
