using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public abstract class PlayerState :  IState {

    protected Player parent;
    protected EventManager eventManager = new EventManager();


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
        parent.SetState(new HurtState(e.player_hit, e.properties, e.launch_dir));
    }

  

   
}
