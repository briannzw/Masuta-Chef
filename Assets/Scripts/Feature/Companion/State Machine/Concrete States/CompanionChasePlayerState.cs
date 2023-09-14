using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionChasePlayerState : CompanionState
{
    public CompanionChasePlayerState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine){}

    public override void AnimationTriggerEvent(Companion.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        companion.Agent.isStopped = false;
        
        Debug.Log("Companion Chase Player");
    }

    public override void ExitState()
    {
        base.ExitState();
        companion.Agent.isStopped = true;
    }

    public override void FrameUpdate()
    {
        //Debug.Log("Is Chasing PLayer");
        base.FrameUpdate();
        Debug.Log(companion.Agent.remainingDistance);
        companion.UpdateAgentDestination(EnemySpawner.Instance.PlayerPosition.gameObject);
        if (companion.Agent.remainingDistance <= companion.MinDistanceTowardsPlayer)
        {
            companion.StateMachine.ChangeState(companion.CompanionIdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
