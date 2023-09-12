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
    }

    public override void ExitState()
    {
        base.ExitState();
        companion.Agent.isStopped = true;
    }

    public override void FrameUpdate()
    {
        Debug.Log("Is Chasing PLayer");
        base.FrameUpdate();
        companion.Agent.SetDestination(EnemySpawner.Instance.PlayerPosition.position);
        Vector3 velocity = companion.Agent.velocity;
        if (velocity.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            companion.transform.rotation = Quaternion.Slerp(companion.transform.rotation, targetRotation, Time.deltaTime * companion.Agent.angularSpeed);
        }
        if (companion.Agent.remainingDistance < companion.MaxDistanceTowardsPlayer)
        {
            companion.Agent.isStopped = true;
        }
        else
        {
            companion.Agent.isStopped = false;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
