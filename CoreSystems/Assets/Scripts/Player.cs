using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    public float horizontal_speed_max;
    public float horizontal_acceleration;
    public float horizontal_drag;
    public float jump_height_max;
    public float jump_height_min;
    public float initial_distance_to_peak;
    public float final_distance_to_peak;

    public LayerMask _collisionMask;
    
    public Vector2 analogVector;
    public PlayerState mState;
    private PlayerState previousState;

    private Rigidbody2D _rigidbody2D; 
    public BoxCollider2D _physicsCollider { get; private set; }

    public Vector2 _velocity;
    public float gravity;

    private const float NEAR_ZERO = .0001f;

    // Use this for initialization
    void Start () {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _physicsCollider = this.GetComponent<BoxCollider2D>();

        mState = new IdleState(this);

        gravity = PhysX.CalculateGravity(jump_height_max, initial_distance_to_peak, horizontal_speed_max);
	}
	
	// Update is called once per frame
	void Update () {
        mState = mState.HandleTransitions();

	}

    private void FixedUpdate()
    {
        Debug.Log(mState.GetType());
        mState.Tick();
        UpdatePosition();
    }

    private void UpdateState()
    {
        previousState = mState;
        mState = mState.HandleTransitions();
    }

    private void UpdatePosition()
    {
        _rigidbody2D.MovePosition((Vector2)transform.position +
            new Vector2(_velocity.x * Time.deltaTime + horizontal_acceleration / 2 * Time.deltaTime * Time.deltaTime,
                         _velocity.y * Time.deltaTime + gravity / 2 * Time.deltaTime * Time.deltaTime));
    }


}
