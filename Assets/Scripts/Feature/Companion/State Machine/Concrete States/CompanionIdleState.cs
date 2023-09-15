using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionIdleState : CompanionState
{
    public CompanionIdleState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine){}

    public override void AnimationTriggerEvent(Companion.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        companion.Agent.isStopped = true;
        companion.OnNearestEnemyChanged += HandleNearestEnemyChanged;
        Debug.Log("Enter Companion Idle State");
    }

    public override void ExitState()
    {
        base.ExitState();
        companion.OnNearestEnemyChanged -= HandleNearestEnemyChanged;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        if (companion.Agent.remainingDistance >= companion.MaxIdleDistance)
        {
            companion.StateMachine.ChangeState(companion.CompanionChasePlayerState);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void HandleNearestEnemyChanged(GameObject newNearestEnemy)
    {
        if (newNearestEnemy != null)
        {
            companion.StateMachine.ChangeState(companion.CompanionChaseEnemyState);
        }
    }
}
