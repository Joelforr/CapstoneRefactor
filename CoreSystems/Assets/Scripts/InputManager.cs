using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public delegate PlayerState TransitionState(PlayerState currentState, Input input);
    public event TransitionState TransitionHandler;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateState()
    {
        TransitionHandler();
    }
}
