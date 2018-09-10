using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;
using EventList;

public class HurtState : PlayerState{
    private float attack_damage;                //optimal value(ex, 3-11)
    private float knockback_growth;             //optimal value (ex, 1.1)
    private float victim_percent;              
    private float base_knockback;               //optimal value (ex, 0-3)
    private const float weight_const = 1.5f;    //optimal value (ex, 1.5)

    private float launch_velocity;
    private float launch_angle;
    private float launch_direction;
    private float victim_launch_res;
    private int stun_duration;

    private Vector2 impulse_direction;


    public HurtState(FSM parent)
    {
        this.sm = parent;
    }

    public HurtState(FSM parent, CollisionBoxData properties, float launch_direction)
    {
        eventManager = new EventManager();
        this.sm = parent;
        this.attack_damage = properties.attack_damage;
        this.knockback_growth = properties.knockback_growth;
        this.victim_percent = (1f - parent._character._stamina.GetPercentage())*100f;
        this.base_knockback = properties.base_knockback;
        this.launch_angle = properties.launch_angle;
        this.launch_direction = launch_direction;
        OnStateEnter();
    }


    public override PlayerState HandleTransitions()
    {
        if(stun_duration < 0)
        {
            return new FallState(sm);
        }
        else
        {
            return this;
        }

    }

    public override void OnStateEnter()
    {
        //Handler Setup
        eventManager.AddHandler<CollisionEvent>(OnCollision);
        eventManager.AddHandler<WallInteractionEvent>(OnWallInteraction);
        eventManager.AddHandler<HitEvent>(OnHit);
        
        //Set Animation
        sm._character._xAnimator.SetAnimation(Services.AnimationLibray.GetXAnimation(sm.c, AnimationLibrary.AnimationTags.Hurt) as XAnimation);

        launch_velocity = CalculateLaunchVelocity();
        sm._character._velocity.x = Mathf.Cos(Mathf.Deg2Rad * launch_angle) * launch_velocity * Mathf.Sign(launch_direction);
        sm._character._velocity.y = Mathf.Sin(Mathf.Deg2Rad * launch_angle) * launch_velocity;
        impulse_direction = sm._character._velocity.normalized;
        eventManager.Fire(new WallInteractionEvent(sm._character, sm._character.colliding_against));

        stun_duration = Mathf.FloorToInt(launch_velocity * .5f);
    }

    public override void OnStateExit()
    {
        eventManager.RemoveHandler<HitEvent>(OnHit);
        eventManager.RemoveHandler<AnimationCompleteEvent>(OnAnimationComplete);
    }

    public override void Tick()
    {
        sm._character._velocity.x = sm._character._velocity.normalized.x * Mathf.MoveTowards(sm._character._velocity.magnitude, 0 ,Mathf.Cos(Mathf.Deg2Rad * launch_angle)*(15f * Time.deltaTime));
        //parent._velocity.x = Mathf.Max(parent._velocity.x, 0);
        //parent._velocity.y -= Mathf.Sin(Mathf.Deg2Rad * launch_angle)*(launchResistance * Time.deltaTime);
        sm._character.GravityTick();
        stun_duration--;

        sm._character._velocity += sm._character.directionalInput * sm._character.influenceDrag;
    }

    void OnCollision(CollisionEvent e)
    {
        //Debug.Log(parent._velocity.sqrMagnitude);
        Vector2 num = Vector2.zero;

        foreach (ContactPoint2D point in e.collision2D.contacts)
        {
            num += point.normal;
        }

        num /= e.collision2D.contacts.Length;
        //Debug.Log("First normal" + e.collision2D.contacts[0].normal);
        //Debug.Log("Average normal" + num);
        //Debug.Log("Pre Hit Vel: " + parent._velocity);


        if ((sm._character._velocity.sqrMagnitude > 1000))
        {
           
            Vector2 reflection_dir = Vector3.Reflect(sm._character._velocity.normalized, e.collision2D.contacts[0].normal);      
            sm._character._velocity = sm._character._velocity.magnitude * reflection_dir;
            sm._character._velocity -= (sm._character._velocity * .2f);
        }

     
    }

    void OnWallInteraction(WallInteractionEvent e)
    {
        Vector2 reflection_dir;
        Debug.Log(sm._character._velocity.normalized);

        if (e.wall == BaseCharacter.CollidedSurface.Ground)
        {
            if(sm._character._velocity.normalized.y <= -.7f)
            {
                sm._character._velocity.x *= .65f;
            }
            
            
           
        }

        switch (e.wall) {
            case BaseCharacter.CollidedSurface.LeftWall :
                reflection_dir = Vector3.Reflect(sm._character._velocity.normalized, Vector3.right);
                sm._character._velocity = sm._character._velocity.magnitude * reflection_dir;
                sm._character._velocity -= (sm._character._velocity * .2f);
                break;

            default:
                break;
        }

    }


    private float CalculateLaunchVelocity()
    {
        /*
        //Two possible equations
        //Linear Growth:
        //launch_velocity = knockback_growth * (1.4*
        //((udmg+2)*(sdmg+floor(percent))/20) * (2/(1+weight)) + 18) + b
        //
        //Exponential Growth:
        //launch_velocity = knockbakc_growth * (1.4*((udmg + percent^2)/ var_q ))+b
        //where var_q is a replacement for 20 in the linear function 
        //usually > 40
        */

        return knockback_growth * (1.4f * ((attack_damage + 2) * (0 + Mathf.FloorToInt(victim_percent)) / 40) * (2 / (1 + weight_const)) + 18) + base_knockback;
    }

    
}
