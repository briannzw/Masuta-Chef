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
        Debug.Log("Companion Idle");
    }

    public override void ExitState()
    {
        base.ExitState();
        companion.Agent.isStopped = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        companion.UpdateAgentDestination(EnemySpawner.Instance.PlayerPosition.gameObject);
        if (companion.Agent.remainingDistance >= companion.MaxDistanceTowardsPlayer)
        {
            companion.StateMachine.ChangeState(companion.CompanionChasePlayerState);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
