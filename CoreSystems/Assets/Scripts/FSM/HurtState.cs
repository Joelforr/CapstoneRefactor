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


    public HurtState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }

    public HurtState(Player parent, HitboxProperties properties, float launch_direction)
    {
        eventManager = new EventManager();
        this.parent = parent;
        this.attack_damage = properties.attack_damage;
        this.knockback_growth = properties.knockback_growth;
        this.victim_percent = parent.vp;
        this.base_knockback = properties.base_knockback;
        this.launch_angle = properties.launch_angle;
        this.launch_direction = launch_direction;
        OnStateEnter();
    }

    public HurtState(Player parent, float ad, float kg, float vp, float bk, float la)
    {
        this.parent = parent;
        this.attack_damage = ad;
        this.knockback_growth = kg;
        this.victim_percent = vp;
        this.base_knockback = bk;
        this.launch_angle = la;
        OnStateEnter();
    }

    public override void AnimationTransitionEvent()
    {

    }

    public override PlayerState HandleTransitions()
    {
        if(stun_duration < 0)
        {
            return new FallState(parent);
        }
        else
        {
            return this;
        }

    }

    public override void OnStateEnter()
    {
        eventManager.AddHandler<CollisionEvent>(OnCollision);

        parent._xAnimator.SetAnimation(Resources.Load("Data/XAnimationData/Knockback_XAnimation") as XAnimation);
        parent.stamina -= CalculateDamage();
        launch_velocity = CalculateLaunchVelocity();
        parent._velocity.x = Mathf.Cos(Mathf.Deg2Rad * launch_angle) * launch_velocity * Mathf.Sign(launch_direction);
        parent._velocity.y = Mathf.Sin(Mathf.Deg2Rad * launch_angle) * launch_velocity;
        impulse_direction = parent._velocity.normalized;

        stun_duration = Mathf.FloorToInt(launch_velocity * .9f);
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        parent._velocity.x = parent._velocity.normalized.x * Mathf.MoveTowards(parent._velocity.magnitude, 0 ,Mathf.Cos(Mathf.Deg2Rad * launch_angle)*(15f * Time.deltaTime));
        //parent._velocity.x = Mathf.Max(parent._velocity.x, 0);
        //parent._velocity.y -= Mathf.Sin(Mathf.Deg2Rad * launch_angle)*(launchResistance * Time.deltaTime);
        parent.GravityTick();
        stun_duration--;
    }

    void OnCollision(CollisionEvent e)
    {
        Debug.Log(parent._velocity.sqrMagnitude);
        Vector2 num = Vector2.zero;

        foreach (ContactPoint2D point in e.collision2D.contacts)
        {
            num += point.normal;
        }

        num /= e.collision2D.contacts.Length;
        Debug.Log("First normal" + e.collision2D.contacts[0].normal);
        Debug.Log("Average normal" + num);
        Debug.Log("Pre Hit Vel: " + parent._velocity);
        if((parent._velocity.sqrMagnitude > 1000))
        {
           
            Vector2 reflection_dir = Vector3.Reflect(parent._velocity.normalized, e.collision2D.contacts[0].normal);      
            parent._velocity = parent._velocity.magnitude * reflection_dir;
            parent._velocity -= (parent._velocity * .2f);
        }
    }

    private float CalculateDamage()
    {
        return 10;
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

        return knockback_growth * (1.4f * ((attack_damage + 2) * (0 + Mathf.FloorToInt(victim_percent)) / 20) * (2 / (1 + weight_const)) + 18) + base_knockback;
    }

    
}
