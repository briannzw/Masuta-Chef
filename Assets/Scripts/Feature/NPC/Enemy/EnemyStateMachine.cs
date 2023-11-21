using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentState;

    public void Initialize(EnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
