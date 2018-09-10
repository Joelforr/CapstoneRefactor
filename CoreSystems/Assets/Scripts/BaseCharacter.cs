using EventList;
using Rewired;
using System;
using System.Collections;
using UnityEngine;
using Xeo;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseCharacter : MonoBehaviour {

    public int playerId = 0;
    public Player player;

    public float horizontal_attack_speed;
    public float horizontal_attack_drag;
    public float horizontal_air_acceleration;
    public float horizontal_air_speed;
    public float horizontal_speed_max;
    public float horizontal_acceleration;
    public float horizontal_drag;
    public float jump_height_max;
    public float short_jump_height;
    public float initial_distance_to_peak;
    public float final_distance_to_peak;
    public float air_drag;

    public float airJumpHeight;
    public float airDistanceToPeak;
    public float dodgeDistance;

    public float influenceDrag;

    [Range(0f, 1.2f)]
    public float motorLevel;
    [Range(0, 4)]
    public int motorIndex;

    public AnimationLibrary.CharacterTags color;

    public enum PlayerStatus
    {
        Alive,
        Dead,
        Frozen
    }
    public PlayerStatus status = PlayerStatus.Alive;

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

    public CollidedSurface colliding_against { get; private set; }
    private float env_check_dist = 0.04f;

    public Vector2 directionalInput;
    private float facing = 1;

    public FSM _sm{get; private set;}
    public SFXController sfxPlayer;
    private Rigidbody2D _rigidbody2D; 
    public BoxCollider2D _characterCollider { get; private set; }
    public XAnimator _xAnimator;

    public float stamina;
    public float stamina_regen {private get; set; }
    public Stamina _stamina = new Stamina(60f,1/8f);
    public StaminaSystem staminaSystem;

    public Vector2 _velocity;
    public float gravity;

    public float gravity_cutoff = -50f;                            //If the y velocity is below this value stop applying gravity


    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);

        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _characterCollider = this.GetComponent<BoxCollider2D>();
        _xAnimator = this.GetComponent<XAnimator>();
        sfxPlayer = this.GetComponent<SFXController>();
        staminaSystem = this.GetComponent<StaminaSystem>();
    }

    // Use this for initialization
    void Start () {
        _sm = new FSM(this);
        _sm.TransitionTo(new IdleState(_sm), true);
        gravity = PhysX.CalculateGravity(jump_height_max, initial_distance_to_peak, horizontal_speed_max);
	}
	
	// Update is called once per frame
	void Update () {
        if (status == PlayerStatus.Alive)
        {
            UpdateDirectionalInformation();
            _sm.Update();
        }
        colliding_against = CheckSurroundings();
	}

    private void FixedUpdate()
    {
        if (status == PlayerStatus.Alive)
        {
            _sm.FixedUpdate();
            MovePosition();
        }
    }


    private void UpdateDirectionalInformation()
    {
        directionalInput = Vector2.zero;

        float deadzone = .3f;

        Vector2 normalizedInput = player.GetAxis2D(0, 1);
        if (normalizedInput.magnitude > deadzone)
        {
            if (Mathf.Abs(normalizedInput.x) >= deadzone)
            {
                directionalInput.x += Mathf.Sign(normalizedInput.x);
            }

            if (normalizedInput.y != 0)
            {
                directionalInput.y += Mathf.Sign(normalizedInput.y);
            }
        }

        /*directionalInput = player.GetAxis2D(0, 1);
        if (player.GetAxis2D(0, 1).magnitude > deadzone)
        {
            directionalInput = directionalInput.normalized * (directionalInput.magnitude - deadzone) / (1 - deadzone);
        }
        else
        {
            directionalInput = Vector2.zero;
        }*/

        if (Mathf.Abs(directionalInput.x) < deadzone)
        {
            directionalInput.x = 0;
        }
    }


    private void MovePosition()
    {
        _rigidbody2D.MovePosition((Vector2)transform.position +
            new Vector2(_velocity.x * Time.deltaTime + horizontal_acceleration / 2 * Time.deltaTime * Time.deltaTime,
                        _velocity.y * Time.deltaTime + gravity / 2 * Time.deltaTime * Time.deltaTime));
    }

    
    private CollidedSurface CheckSurroundings()
    {
        CollidedSurface surfaces = CollidedSurface.None;

        if(Xeo.Collisions.IsGrounded(_characterCollider, _collisionMask, env_check_dist))
        {
            surfaces |= CollidedSurface.Ground;
        }

        if (Xeo.Collisions.IsAgainstLeftWall(_characterCollider, _collisionMask, env_check_dist))      
        {
            surfaces |= CollidedSurface.LeftWall;
        }

        if (Xeo.Collisions.IsAgainstRightWall(_characterCollider, _collisionMask, env_check_dist))
        {
            surfaces |= CollidedSurface.RightWall;
        }

        return surfaces;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            if(collision.contacts[0].normal == Vector2.up)
            {
                StartCoroutine(IgnoreCollisionBox());
            }
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            _sm.currentState.FireCustomEvent(new CollisionEvent(this, collision));
        }
    }

    private IEnumerator IgnoreCollisionBox()
    {
        _characterCollider.enabled = false;
        yield return new WaitForSeconds(.1f);
        _characterCollider.enabled = true;
    }

    #region public
    public float GetFacing()
    {
        return facing;
    }

    public void GravityTick()
    {
        _velocity.y += _velocity.y <= gravity_cutoff ? 0 : gravity * Time.deltaTime;
    }

    public bool HasFlag(CollidedSurface cs)
    {
        return (colliding_against & cs) != CollidedSurface.None;
    }

    public bool IsPressingIntoLeftWall()
    {
        return HasFlag(CollidedSurface.LeftWall) && directionalInput.x < 0;
    }

    public bool IsPressingIntoRighttWall()
    {
        return HasFlag(CollidedSurface.RightWall) && directionalInput.x > 0;
    }

    public bool IsGrounded()
    {
        return HasFlag(CollidedSurface.Ground);
    }

    public void Reset(Vector2 spawn_Location)
    {
        _stamina.Reset();
        staminaSystem.Reset();
        _velocity = Vector2.zero;
        _sm.TransitionTo(new IdleState(_sm));
        transform.position = spawn_Location;
       

    }

    public void SetFacing()
    {
        if (directionalInput.x != 0) facing = Mathf.Sign(directionalInput.x);
    }
    #endregion
}
