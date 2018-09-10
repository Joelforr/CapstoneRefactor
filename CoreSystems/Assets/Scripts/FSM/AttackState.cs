using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public class AttackState : PlayerState {

    //Temp Vars
    private float timeStart;
    private float frameCount;

    private Vector2 di;

    public enum AttackType
    {
        Ground,
        Air
    }

    private AttackType attack_type;
    private Vector2 analog_dir;

    public AttackState(FSM parent)
    {
        this.sm = parent;
        this.di = sm._character.player.GetAxis2D(0,1);
        this.cost = 10f;
        this.orbCost = 1;
    }

    public AttackState(FSM parent, Vector2 directionalInfluence)
    {
        this.sm = parent;
        this.di = directionalInfluence;
        this.cost = 10f;
        this.orbCost = 1;
    }

    protected override void OnAnimationComplete(AnimationCompleteEvent e)
    {
        switch (attack_type)
        {
            case AttackType.Ground:
                sm.TransitionTo(new IdleState(sm));
                break;

            case AttackType.Air:
                sm.TransitionTo(new FallState(sm));
                break;

            default:
                sm.TransitionTo(new IdleState(sm));
                break;
        }
     
    }

    public override PlayerState HandleTransitions()
    {
        return this;
    }

    public override void OnStateEnter()
    {
        //Handler set-up
        eventManager.AddHandler<HitEvent>(OnHit);
        eventManager.AddHandler<AnimationCompleteEvent>(OnAnimationComplete);

        Debug.Log(Xeo.Math.ToAng(di));
        //Switch statement
        switch (sm.previousState.GetType() == typeof(JumpState) || sm.previousState.GetType() == typeof(FallState))
        {
            case true:
                attack_type = AttackType.Air;
        
                if (Xeo.Math.ToAng(di) >= 50f && Xeo.Math.ToAng(di) <= 130f)
                {
                    sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Attack_U_A) as XAnimation);
                }
                else if (Xeo.Math.ToAng(di) >= 230f && Xeo.Math.ToAng(di) <= 310f)
                {
                    sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Attack_D_A) as XAnimation);
                }
                else
                {
                    sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Attack_S_A) as XAnimation);
                }
                break;
            case false:
                if (Xeo.Math.ToAng(di) >= 50f && Xeo.Math.ToAng(di) <= 130f)
                {
                    sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Attack_U_G) as XAnimation);
                }
                else
                {
                    sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Attack_S_G) as XAnimation);
                }
                break;
            default:
                
                break;
        }

        //Play attack animation based off atk_type & direction
        //Animation handles state switching
        
    }

    public override void OnStateExit()
    {
        //Adjust Cooldowns And Timers
        sm.SetTimer(this.GetType());

        eventManager.RemoveHandler<HitEvent>(OnHit);
        eventManager.RemoveHandler<AnimationCompleteEvent>(OnAnimationComplete);
    }

    public override void Tick()
    {

        switch (attack_type)
        {
            case AttackType.Ground :
                //parent._velocity = Vector2.zero;

                if (Mathf.Abs(sm._character.directionalInput.x) > .5f)       //.5f deadzone
                {
                    sm._character._velocity.x += sm._character.directionalInput.x * sm._character.horizontal_acceleration;

                    if (sm._character._velocity.x > sm._character.horizontal_attack_speed || sm._character._velocity.x < -sm._character.horizontal_attack_speed)
                    {
                        sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, 0, sm._character.horizontal_attack_drag);
                    }
                }
                else
                {
                    sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, 0, sm._character.horizontal_attack_drag);
                }
                break;

            case AttackType.Air:
                sm._character._velocity.y += sm._character.gravity * Time.deltaTime;
                break;

            default:
                break;
        }
    }

}
