using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
public class DodgeState : PlayerState {

    private float distance;
    private int duration;

    private int dashingFrames;



    public DodgeState(FSM sm)
    {
        this.sm = sm;
        this.character = sm._character;
        this.distance = character.dodgeDistance;
        this.orbCost = 1;
    }

    public override PlayerState HandleTransitions()
    {
        if (dashingFrames <= 0)
        {
            return new RunState(sm);
        }
        else
        {
            return this;
        }
    }

    public override void OnStateEnter()
    {

        character._velocity.y = 0;

        character._characterCollider.enabled = false;
        character.SetFacing();

        //Set Animation
        sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Dodge) as XAnimation);

        //Play Audio
        sm._character.sfxPlayer.Play(Services.SFXLibrary.GetSFX(SFXLibrary.SFXTags.Dodge));

        duration = sm._character._xAnimator.GetAnimationLengthFrames();
        dashingFrames = (int)duration - 1;

        //add bonus to distance if character was running
        distance += character._velocity.x * 2 * Time.deltaTime;
    }

    public override void OnStateExit()
    {
        //Adjust Cooldowns And Timers
        sm.SetTimer(this.GetType());

        //Stop Audio
        sm._character.sfxPlayer.Stop();

        character._characterCollider.enabled = true;
    }

    public override void Tick()
    {
        character._velocity.x = GetDashSpeed();
        dashingFrames--;         
    }

    private float GetDashSpeed()
    {
        float normalizedTime = (float)((duration) - dashingFrames) / (duration);

   


        float speed = EaseOutQuadD(0, distance, normalizedTime) / duration / Time.fixedDeltaTime * character.GetFacing();

        // Some of the easing functions may result in infinity, we'll uh, lower our expectations and make it maxfloat.
        // This will almost certainly be clamped.
        if (float.IsNegativeInfinity(speed))
        {
            speed = float.MinValue;
        }
        else if (float.IsPositiveInfinity(speed))
        {
            speed = float.MaxValue;
        }

        return speed;
    }



    private float EaseOutQuadD(float start, float end, float value)
    {
        end -= start;
        return -end * value - end * (value - 2);
    }

}
