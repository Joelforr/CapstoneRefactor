using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xeo;

public class AttackState : PlayerState {

    public enum AttackType
    {
        Ground,
        Air
    }

    private AttackType attack_type;

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

    public override PlayerState HandleTransitions()
    {
        return this;
    }

    public override void OnStateEnter()
    {
        //Switch statement
        //Play attack animation based off atk_type & direction
        //Animation handles state switching
        
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
       
    }

}
