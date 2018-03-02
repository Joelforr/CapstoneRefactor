using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

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
    [Flags]
    public enum CollidedSurface
    {
        None = 0x0,
        Ground = 0x1,
        LeftWall = 0x2,
        RightWall = 0x4,
        Cieling = 0x8
    }

    private CollidedSurface colliding_against;
    private float env_check_dist = 0.04f;

    public Vector2 normalized_directional_input;
    public float facing_direction;

    public PlayerState mState;
    public PlayerState previousState { get; private set;}

    private Rigidbody2D _rigidbody2D; 
    public BoxCollider2D _physicsCollider { get; private set; }
    public XAnimator _xAnimator;
    public InputManager _inputManager;

    public float max_stamina;
    public float stamina;
    public float stamina_regen;
    public float stamina_precentage;

    public Vector2 _velocity;
    private Vector2 _integratedVelocity;
    public float gravity;

    private float gravity_cutoff = -50f;                            //If the y velocity is below this value stop applying gravity

    public float ad, kg, vp, bk, la;


    // Use this for initialization
    void Start () {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _physicsCollider = this.GetComponent<BoxCollider2D>();
        _xAnimator = this.GetComponent<XAnimator>();
        _inputManager = this.GetComponent<InputManager>();

        mState = new IdleState(this);
        gravity = PhysX.CalculateGravity(jump_height_max, initial_distance_to_peak, horizontal_speed_max);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateDirectionalInformation();
        _xAnimator.SetFacing(facing_direction);
        //mState = mState.HandleTransitions();
        UpdateState();
        colliding_against = CheckSurroundings();

        //Debug.Log(mState);
        Debug.Log(colliding_against);
	}

    private void FixedUpdate()
    {
        mState.Tick();
        UpdatePosition();
    }

    private void UpdateDirectionalInformation()
    {
        this.normalized_directional_input = Vector2.zero;

        if (_inputManager.controller.magnitude > .3f)
        {
            if(_inputManager.controller.x != 0)
            {
                normalized_directional_input.x += Mathf.Sign(_inputManager.controller.x);
            }

            if (_inputManager.controller.y != 0)
            {
                normalized_directional_input.y += Mathf.Sign(_inputManager.controller.y);
            }
        }

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
  

    public void GravityTick()
    {
        _velocity.y += _velocity.y <= gravity_cutoff ? 0 : gravity * Time.deltaTime; 
    }

    public bool HasFlag(CollidedSurface cs)
    {
        return (colliding_against & cs) != CollidedSurface.None;
    }

    public void RegenStamina()
    {
        stamina += stamina_regen;
        stamina = Mathf.Clamp(stamina, 0, max_stamina);
    }

    private CollidedSurface CheckSurroundings()
    {
        CollidedSurface surfaces = CollidedSurface.None;

        if(Xeo.Collisions.IsGrounded(_physicsCollider, _collisionMask, env_check_dist))
        {
            surfaces |= CollidedSurface.Ground;
        }

        if (Xeo.Collisions.IsAgainstLeftWall(_physicsCollider, _collisionMask, env_check_dist) && 
            normalized_directional_input.x < 0)
        {
            surfaces |= CollidedSurface.LeftWall;
        }

        if (Xeo.Collisions.IsAgainstRightWall(_physicsCollider, _collisionMask, env_check_dist) &&
            normalized_directional_input.x > 0)
        {
            surfaces |= CollidedSurface.RightWall;
        }

        return surfaces;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(_velocity.sqrMagnitude);
        if(collision.gameObject.layer == LayerMask.NameToLayer("Physx") && _velocity.sqrMagnitude > 800f)
        {
            Physics2D.IgnoreCollision(_physicsCollider, collision.collider, true);
        }
        else
        {
            Physics2D.IgnoreCollision(_physicsCollider, collision.collider, false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            mState.FireCustomEvent(new CollisionEvent(this, collision));
        }
    }
}
