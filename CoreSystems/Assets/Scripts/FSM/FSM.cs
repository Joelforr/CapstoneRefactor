using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FSM{

    public readonly BaseCharacter _character;

    public PlayerState currentState;
    public PlayerState previousState;

    public int attackGraceFrames =-1;
    public int dodgeGraceFrames =-1;
    public int jumpGraceFrames =-1;

    private int attackCooldown = 10;
    private int dodgeCooldown = 5;
    private int jumpCooldown = 5;

    public int jumpCount = 0;
    public int jumpsAllowed = 1000;

    public AnimationLibrary.CharacterTags c;

    public FSM(BaseCharacter player)
    {
        _character = player;
        c = _character.color;
    }

    //WANT TO CHECK FOR TRANSITIONS EVERY UPDATE LOOP
    public void Update()
    {
        //if (_character.IsGrounded()) jumpCount = 0;
        TransitionCheck();
	}

    //WANT TO CALL THE TICK METHOD ON CURRENT STATE EVERY FIXED UPDATE LOOP
    public void FixedUpdate()
    {
        currentState.Tick();
        UpdateTimers();
    }

    public void SetTimer(Type type)
    {
        if(type == typeof(AttackState))
        {
            attackGraceFrames = attackCooldown;
            return;
        }

        if (type == typeof(DodgeState))
        {
            dodgeGraceFrames = dodgeCooldown;
            return;
        }

        if (type == typeof(JumpState))
        {
            jumpGraceFrames = jumpCooldown;
            jumpCount++;
            return;
        }

        return;

    }

    /// <summary>
    /// Checks if we should be transitioning to a different state
    /// </summary>
    public void TransitionCheck()
    {
        if (currentState != null)
        {
            TransitionTo(currentState.HandleTransitions());
        }
    }

    /// <summary>
    /// Transition to a new state.
    /// </summary>
    /// <param name="newState">The state we will be transitioning to</param>
    /// <param name="force">Force the transition</param>
    public void TransitionTo(PlayerState newState, bool force = false)
    {
        if (newState == currentState) return;


        if ((newState.orbCost <= _character.staminaSystem.GetCurrentStamina()) || force == true)
        {
            _character._stamina.Current -= newState.cost;

            
            _character.staminaSystem.SpendOrbs(newState.orbCost);

            if (currentState != null)
            {
                currentState.OnStateExit();
                previousState = currentState;
            }

            currentState = newState;
            newState.OnStateEnter();
        }
        else
        {
            _character.player.SetVibration(1, .8f, .5f, true);
            Debug.Log("Not enough stamina for transition");
        }
    }

    private void UpdateTimers()
    {
        attackGraceFrames--;
        dodgeGraceFrames--;
        jumpGraceFrames--;
    }
}
