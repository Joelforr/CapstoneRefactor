using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public class AttackState : PlayerState {

    //Temp Vars
    private float timeStart;
    private float frameCount;

    public enum AttackType
    {
        Ground,
        Air
    }

    private AttackType attack_type;
    private Vector2 analog_dir;

    public AttackState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }

    public AttackState(Player parent, AttackType atk_type)
    {
        this.parent = parent;
        this.attack_type = atk_type;
        OnStateEnter();
    }

    protected override void OnAnimationComplete(AnimationCompleteEvent e)
    {
        switch (attack_type)
        {
            case AttackType.Ground:
                parent.SetState(new IdleState(parent));
                break;

            case AttackType.Air:
                parent.SetState(new FallState(parent));
                break;

            default:
                parent.SetState(new IdleState(parent));
                break;
        }
     
    }

    public override PlayerState HandleTransitions()
    {
        return this;
    }

    public override void OnStateEnter()
    {
        //Handler set-up
        eventManager.AddHandler<HitEvent>(OnHit);
        eventManager.AddHandler<AnimationCompleteEvent>(OnAnimationComplete);

        timeStart = Time.time;
        Debug.Log("TimeS: " + timeStart);
        Debug.Log("TimeG: " + (timeStart + (7f/60f)));

        //Switch statement
        //Play attack animation based off atk_type & direction
        //Animation handles state switching

        //parent.stamina -= 15f;
        parent._xAnimator.SetAnimation(Resources.Load("Data/XAnimationData/F_Slash_XAnimation") as XAnimation);

    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        Debug.Log(Time.time);
        if (Time.time > (timeStart + (7f / 60f)))
        {
            if (Input.GetButtonDown(parent._inputManager.jump)){
                Debug.Log("Tried to jump");
                parent.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 100f));
            }
        }

        switch (attack_type)
        {
            case AttackType.Ground :
                //parent._velocity = Vector2.zero;

                if (Mathf.Abs(parent.normalized_directional_input.x) > .5f)       //.5f deadzone
                {
                    parent._velocity.x += parent.normalized_directional_input.x * parent.horizontal_acceleration;

                    if (parent._velocity.x > parent.horizontal_attack_speed || parent._velocity.x < -parent.horizontal_attack_speed)
                    {
                        parent._velocity.x = parent._velocity.normalized.x * Mathf.MoveTowards(parent._velocity.magnitude, 0, parent.horizontal_attack_drag);
                    }
                }
                else
                {
                    parent._velocity.x = parent._velocity.normalized.x * Mathf.MoveTowards(parent._velocity.magnitude, 0, parent.horizontal_attack_drag);
                }
                break;

            case AttackType.Air:
                parent._velocity.y += parent.gravity * Time.deltaTime;
                break;

            default:
                break;
        }
    }

}
