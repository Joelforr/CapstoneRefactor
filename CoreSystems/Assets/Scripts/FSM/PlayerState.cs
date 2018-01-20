using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState :  IState {

    protected Player parent;


    public abstract PlayerState HandleTransitions();

    public abstract void Tick();

    public abstract void OnStateEnter();
    public abstract void OnStateExit();
}
