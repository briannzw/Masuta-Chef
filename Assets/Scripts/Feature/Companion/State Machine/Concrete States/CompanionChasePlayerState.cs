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
        companion.OnNearestEnemyChanged += HandleNearestEnemyChanged;
        Debug.Log("Entering State Companion Chase Player");
    }

    public override void ExitState()
    {
        base.ExitState();
        companion.Agent.isStopped = true;
        companion.OnNearestEnemyChanged -= HandleNearestEnemyChanged;
    }

    public override void FrameUpdate()
    {
        //Debug.Log("Is Chasing PLayer");
        base.FrameUpdate();
        companion.UpdateAgentDestination(GameManager.Instance.PlayerGameObject.gameObject);
        if (companion.Agent.remainingDistance <= companion.MinDistanceTowardsPlayer)
        {
            companion.StateMachine.ChangeState(companion.CompanionIdleState);
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
            companion.UpdateAgentDestination(newNearestEnemy);
            companion.StateMachine.ChangeState(companion.CompanionChaseEnemyState);
        }
    }
}
