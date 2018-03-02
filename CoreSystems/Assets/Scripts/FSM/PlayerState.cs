using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;

public abstract class PlayerState :  IState {

    protected Player parent;
    protected EventManager eventManager = new EventManager();


    public abstract PlayerState HandleTransitions();

    public abstract void Tick();

    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void AnimationTransitionEvent();

    public void FireCustomEvent(GameEvent e)
    {
        eventManager.Fire(e);
    }
}
