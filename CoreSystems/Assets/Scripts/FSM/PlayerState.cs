using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public abstract class PlayerState :  IState {

    protected FSM sm;
    protected EventManager eventManager = new EventManager();
    public float cost = 0;
    public int orbCost = 0;

    protected BaseCharacter character;

    public abstract PlayerState HandleTransitions();

    public abstract void Tick();

    public abstract void OnStateEnter();
    public abstract void OnStateExit();

    public void FireCustomEvent(GameEvent e)
    {
        eventManager.Fire(e);
    }

    protected virtual void OnAnimationComplete(AnimationCompleteEvent e) {
           
    }

    protected virtual void OnHit(HitEvent e)
    {
        sm.TransitionTo(new HurtState(e.player_hit._sm, e.properties, e.launch_dir));
    }

  

   
}
