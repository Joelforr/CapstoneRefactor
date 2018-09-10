using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public class JumpState: PlayerState {

    private bool pressed;
    private bool held;
    

    

    public JumpState(FSM parent)
    {
        this.sm = parent;
        this.cost = 3f;

        this.orbCost = (sm._character.IsGrounded()) ? 0 : 1;
        pressed = true;
        held = true;
    }


    public override PlayerState HandleTransitions()
    {

        if(sm._character._velocity.y < 0)
        {
            return new FallState(sm);
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
        //Adjust Cooldowns And Timers
        //Because you can transition from JumpState to JumpState this  
        //is set in OnStateEnter and is slightly longer
        sm.SetTimer(this.GetType());

        //Handler Setup
        eventManager.AddHandler<CollisionEvent>(OnCollision);
        eventManager.AddHandler<HitEvent>(OnHit);

        //Set Animation
        sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Jump) as XAnimation);


        //Play Audio
        sm._character.sfxPlayer.Play(Services.SFXLibrary.GetSFX(SFXLibrary.SFXTags.Jump));

        //Pre-Calculations
        if (sm.jumpCount <= 1)
        {
            sm._character.gravity = PhysX.CalculateGravity(sm._character.jump_height_max, sm._character.initial_distance_to_peak, sm._character.horizontal_air_speed);
            sm._character._velocity.y = PhysX.CalculateJumpVelocity(sm._character.jump_height_max, sm._character.initial_distance_to_peak, sm._character.horizontal_air_speed);
        }
        else
        {
            sm._character.gravity = PhysX.CalculateGravity(sm._character.airJumpHeight, sm._character.airDistanceToPeak, sm._character.horizontal_air_speed);
            sm._character._velocity.y = PhysX.CalculateJumpVelocity(sm._character.airJumpHeight, sm._character.airDistanceToPeak, sm._character.horizontal_air_speed);
        }
    }

    public override void OnStateExit()
    {
        //Set Animation
        sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Fall) as XAnimation);


        //Stop Audio
        sm._character.sfxPlayer.Stop();

        eventManager.RemoveHandler<HitEvent>(OnHit);

    }

    public override void Tick()
    {
        sm._character.SetFacing();

        sm._character._velocity.y += sm._character.gravity * Time.deltaTime;

        if (sm._character.IsPressingIntoLeftWall() || sm._character.IsPressingIntoRighttWall())
        {
            sm._character._velocity.x = 0f;
        }
        else
        {
            if (Mathf.Abs(sm._character.directionalInput.x) > .2f)
            {
                sm._character._velocity.x += sm._character.directionalInput.x * sm._character.horizontal_air_acceleration;


                if (sm._character._velocity.x > sm._character.horizontal_speed_max)
                {
                    sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, sm._character.horizontal_speed_max, sm._character.air_drag);
                }

                if(sm._character._velocity.x < -sm._character.horizontal_speed_max)
                {
                    sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, -sm._character.horizontal_speed_max, sm._character.air_drag);
                }

                //parent._velocity.x = Mathf.Max(Mathf.Min(parent._velocity.x, parent.horizontal_air_speed), -parent.horizontal_air_speed);
            }
            else
            {
                sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, 0, sm._character.air_drag);
            }
        }
    }

    void OnCollision(CollisionEvent e)
    {

        if (e.collision2D.contacts[0].normal == Vector2.down)
        {
            sm._character._velocity.y = 0;
        }

    }
}
