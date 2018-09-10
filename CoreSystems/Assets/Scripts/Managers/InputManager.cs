using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public string controllerHor;
    public string controllerVert;

    public string jump;
    public string fire;

    public Vector2 controller { get; private set; }

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        controller = new Vector2(Input.GetAxis(controllerHor), Input.GetAxis(controllerVert));
	}

}
