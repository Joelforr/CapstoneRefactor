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
    public float short_jump_height;
    public float initial_distance_to_peak;
    public float final_distance_to_peak;
    public float air_drag;

    public LayerMask _collisionMask;

    public Vector2 normalized_directional_input;
    public float facing_direction;

    public PlayerState mState;
    public PlayerState previousState { get; private set;}

    private Rigidbody2D _rigidbody2D; 
    public BoxCollider2D _physicsCollider { get; private set; }
    public XAnimator _xAnimator;

    public float max_stamina;
    public float stamina;
    public float stamina_regen;
    private float stamina_precentage;

    public Vector2 _velocity;
    private Vector2 _integratedVelocity;
    public float gravity;

    public float ad, kg, vp, bk, la;

    private const float NEAR_ZERO = .0001f;

    // Use this for initialization
    void Start () {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _physicsCollider = this.GetComponent<BoxCollider2D>();
        _xAnimator = this.GetComponent<XAnimator>();

        mState = new IdleState(this);

        gravity = PhysX.CalculateGravity(jump_height_max, initial_distance_to_peak, horizontal_speed_max);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateDirectionalInformation();
        _xAnimator.SetFacing(facing_direction);
        //mState = mState.HandleTransitions();
        UpdateState();
	}

    private void FixedUpdate()
    {
        mState.Tick();
        UpdatePosition();
    }

    private void UpdateDirectionalInformation()
    {
        this.normalized_directional_input = Vector2.zero;
        if (Input.GetKey(KeyCode.RightArrow)) normalized_directional_input.x += 1;
        if (Input.GetKey(KeyCode.LeftArrow)) normalized_directional_input.x -= 1;
        if (Input.GetKey(KeyCode.UpArrow)) normalized_directional_input.y += 1;
        if (Input.GetKey(KeyCode.DownArrow)) normalized_directional_input.y -= 1;

        if (normalized_directional_input.x != 0) facing_direction = normalized_directional_input.x;
    }

    private void UpdateState()
    {
        previousState = mState;
        mState = mState.HandleTransitions();
    }

    private void UpdatePosition()
    {
        //_integratedVelocity.x = _velocity.x * Time.deltaTime + horizontal_acceleration / 2 * Time.deltaTime * Time.deltaTime;
        //_integratedVelocity.y = _velocity.y * Time.deltaTime + gravity / 2 * Time.deltaTime * Time.deltaTime;
        //Debug.Log(_integratedVelocity);

        _rigidbody2D.MovePosition((Vector2)transform.position +
            new Vector2(_velocity.x * Time.deltaTime + horizontal_acceleration / 2 * Time.deltaTime * Time.deltaTime,
                        _velocity.y * Time.deltaTime + gravity / 2 * Time.deltaTime * Time.deltaTime));
    }

    public void SetState(PlayerState new_state)
    {
        mState = new_state;
    }

    public void XAnimatorCompletetionCall()
    {
        mState.AnimationTransitionEvent();
    }

    public void UpdateStaminaPrecentage()
    {
        stamina = Mathf.Clamp(stamina, 0, max_stamina);
        stamina_precentage = stamina / max_stamina;
    }

    public void RegenStamina()
    {
        stamina += stamina_regen;
        stamina = Mathf.Clamp(stamina, 0, max_stamina);
    }
}
