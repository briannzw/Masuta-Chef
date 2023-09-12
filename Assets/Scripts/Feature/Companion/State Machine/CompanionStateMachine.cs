using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionStateMachine
{
    public CompanionState CurrentCompanionState { get; set; }

    public void Initialize(CompanionState startingState)
    {
        CurrentCompanionState = startingState;
        CurrentCompanionState.EnterState();
    }

    public void ChangeState(CompanionState newState)
    {
        CurrentCompanionState.ExitState();
        CurrentCompanionState = newState;
        CurrentCompanionState.EnterState();
    }
}
