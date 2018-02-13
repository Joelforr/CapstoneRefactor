using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : PlayerState{

    private float launchForce;
    private float launchResistance;
    private float launchAngle;


    private float attack_damage;                //optimal value(ex, 3-11)
    private float knockback_growth;             //optimal value (ex, 1.1)
    private float victim_percent;              
    private float base_knockback;               //optimal value (ex, 0-3)
    private const float weight_const = 1.5f;    //optimal value (ex, 1.5)

    private float launch_velocity;
    private float launch_angle;
    private float victim_launch_res;
    private int stun_duration;

    public HurtState(Player parent)
    {
        this.parent = parent;
        OnStateEnter();
    }

    public HurtState(Player parent, float launchForce, float launchResist, float launchAngle)
    {
        this.parent = parent;
        this.launchForce = launchForce;
        this.launchResistance = launchResist;
        this.launchAngle = launchAngle;
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
        launch_velocity = CalculateLaunchVelocity();

        parent._velocity.x = Mathf.Cos(Mathf.Deg2Rad * launch_angle) * launch_velocity;
        parent._velocity.y = Mathf.Sin(Mathf.Deg2Rad * launch_angle) * launch_velocity;

        stun_duration = Mathf.FloorToInt(launch_velocity * .9f);
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        //parent._velocity.x -= Mathf.Cos(Mathf.Deg2Rad * launch_angle)*(launchResistance * Time.deltaTime);
        parent._velocity.x = Mathf.Max(parent._velocity.x, 0);
        //parent._velocity.y -= Mathf.Sin(Mathf.Deg2Rad * launch_angle)*(launchResistance * Time.deltaTime);
        parent._velocity.y += parent.gravity * Time.deltaTime;
        stun_duration--;
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
