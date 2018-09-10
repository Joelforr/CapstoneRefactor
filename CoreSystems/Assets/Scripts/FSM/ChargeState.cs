using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : PlayerState {

    private float chargeBonus;              //Bouns gained from charging attack
    private float maxChargeBonus;           //Max charge Bonus
    private Vector2 DI;                     //Directional influence

    public ChargeState(FSM parent, Vector2 directionalInput)
    {

    }
    public override PlayerState HandleTransitions()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        throw new System.NotImplementedException();
    }

}
