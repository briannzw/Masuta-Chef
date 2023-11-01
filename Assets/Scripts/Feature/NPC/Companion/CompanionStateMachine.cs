using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionStateMachine
{
    public CompanionState CurrentState;

    public void Initialize(CompanionState startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    public void ChangeState(CompanionState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
