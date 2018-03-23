using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState  {
    PlayerState HandleTransitions();
    void Tick();
    void OnStateEnter();
    void OnStateExit();

}
