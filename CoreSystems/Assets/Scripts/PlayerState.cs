using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState :  IState {

    protected Player parent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract PlayerState HandleTransitions();

    public abstract void Tick();

    public abstract void OnStateEnter();
    public abstract void OnStateExit();
}
