using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    public float horizontal_speed_max;
    public float horizontal_acceleration;
    public float horizontal_drag;
    public float jump_height_max;
    public float jump_height_min;
    public float initial_distance_to_peak;
    public float final_distance_to_peak;

    public LayerMask envLayerMask;

    public float moveSpd;
    public float maxMoveSpd;
    
    public Vector2 analogVector;
    public PlayerState mState;

    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _physicsCollider;
    public Vector2 _velocity;
    public float gravity;

    private const float NEAR_ZERO = .0001f;

    // Use this for initialization
    void Start () {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _physicsCollider = this.GetComponent<BoxCollider2D>();
        mState = new IdleState(this);
	}
	
	// Update is called once per frame
	void Update () {
        mState = mState.HandleTransitions();

	}

    private void FixedUpdate()
    {
        mState.Tick();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        _rigidbody2D.MovePosition((Vector2)transform.position +
            new Vector2(_velocity.x * Time.deltaTime + horizontal_acceleration / 2 * Time.deltaTime * Time.deltaTime,
                         _velocity.y * Time.deltaTime + gravity / 2 * Time.deltaTime * Time.deltaTime));
    }

    private void SetGrounded()
    {
        //Bottom Left
        Vector2 bl = transform.TransformPoint(_physicsCollider.offset + new Vector2(-_physicsCollider.size.x / 2, -_physicsCollider.size.y / 2));
        //Bottom Right
        Vector2 br = transform.TransformPoint(_physicsCollider.offset + new Vector2(_physicsCollider.size.x / 2, -_physicsCollider.size.y / 2));

        //Ground Check
        if (Physics2D.OverlapArea(bl, br, envLayerMask) != null);

    }

    public float CalculateGravity(float jump_height, float lateral_distance, float x_velocity)
    {
        float gravity;

        gravity = -2 * jump_height * (x_velocity * x_velocity) / ((lateral_distance / 2) * (lateral_distance / 2));

        return gravity;

    }

    public float CalculateJumpVelocity(float jump_height, float lateral_distance, float x_velocity)
    {
        float y_velocity;

        y_velocity = (2 * jump_height * x_velocity) / (lateral_distance / 2);

        return y_velocity;
    }

  

}
